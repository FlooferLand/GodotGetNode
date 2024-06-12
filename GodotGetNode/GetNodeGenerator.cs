using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Originally based on https://github.com/godotengine/godot-proposals/issues/2425#issuecomment-1373034221 (thank you)
// Will be rewritten to be faster/smarter, sometime.

[Generator]
#pragma warning disable CA1050
public class GetNodeGenerator : ISourceGenerator {
#pragma warning restore CA1050
    public void Execute(GeneratorExecutionContext context) {
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            return;
        
        // Generating the code
        foreach (var getNodes in receiver.WorkItems) {
            var result = new StringBuilder();
            string[] names = getNodes.Key.Split('.');
            string spaceName = names[0];
            string className = names[1];
            bool isClassGeneric = className.Contains('<');
            string baseClassName = isClassGeneric ? className.Split('<')[0] : className;
            result.AppendLine($"namespace {spaceName};");
            result.AppendLine("using Godot;");
            result.AppendLine("using System;");
            
            #region The Weird-Looking Code I'll Never Touch Again
            result.AppendLine($$"""

                                partial class {{className}} {
                                    public {{baseClassName}}() {
                                        TreeEntered += () => {
                                """);
            getNodes.Value?.ToList().ForEach(v => result.AppendLine($"\t\t {v.Trim()}"));
            result.AppendLine("""
                              
                                      };
                                  }
                              }
                              """);
            #endregion
            
            // Saving the code
            string fileName = $"{baseClassName}.g.cs";
            string parsed = ($"\n{string.Join("\n", receiver.CodeLog)}\n" + result).Trim();
            if (Debugger.IsAttached) {
                string parsedLc = $"---- {fileName} ----\n";
                int i = 0;
                foreach (string line in parsed.Split('\n')) {
                    parsedLc += $"[{i}] {line}\n";
                    i += 1;
                }
                parsedLc += "\n\n";
            
                // [Put a break-point here in case of emergency!]
                Console.WriteLine(parsedLc);
            }
            context.AddSource(fileName, SourceText.From(parsed, Encoding.UTF8));
        }
    }

    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForPostInitialization(_ => {});
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
}

public class SyntaxReceiver : ISyntaxContextReceiver {
    public List<string> CodeLog { get; } = [];
    public readonly Dictionary<string, List<string>?> WorkItems = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        try {
            if (context.Node is not FieldDeclarationSyntax fieldDeclarationSyntax)
                return;
            foreach (var variableName in fieldDeclarationSyntax.Declaration.Variables) {
                var testClass = (IFieldSymbol)context.SemanticModel.GetDeclaredSymbol(variableName)!;
                var attribute = testClass.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == "GetNode");
                string getNodeCaller = "";
                if (attribute != null) {
                    string? type = testClass.ContainingType.ToString();
                    List<string>? fields = [];
                    if (type != null && !WorkItems.TryGetValue(type, out fields)) {
                        fields = [];
                        WorkItems.Add(type, fields);
                    }

                    // Parsing special referring syntax
                    string? nodeParent = attribute.ConstructorArguments.Length >= 2
                        ? $"{attribute.ConstructorArguments.First().Value}"
                        : null;
                    string nodePath = attribute.ConstructorArguments.Length == 0
                        ? testClass.Type.Name
                        : $"{attribute.ConstructorArguments.Last().Value}";
                    string finalGetNodePath = nodePath;
                    
                    #region deprecated / unpreferred
                    if (nodePath.Contains('{') && nodePath.Contains('}')) {
                        // Getting the referenced variable's name
                        string referencedVarName = "";
                        bool insideBlock = false;
                        foreach (char c in nodePath) {
                            if (insideBlock && !"{}".Contains(c)) {
                                referencedVarName += c;
                            }

                            insideBlock = c switch {
                                '{' => true,
                                '}' => false,
                                _ => insideBlock
                            };
                        }
                            
                        // TODO: Checking if the variable exists
                        // var parent = input;
                        // CodeLog.Add($"// START: {parent}");
                        // CodeLog.Add($"// END\n");
                        
                        // Removing the reference from the input
                        finalGetNodePath = nodePath.Replace($"{{{referencedVarName}}}/", "");
                            
                        // GetNode-ing it
                        getNodeCaller = $"{referencedVarName}";
                        
                        // Deprecation warning
                        string deprecationNotice = $"Use `[GetNode(nameof({referencedVarName}), \"{finalGetNodePath}\")]` instead";
                        Diagnostic.Create(new DiagnosticDescriptor(
                            "UnsafeDeprecatedNodeReference",
                            deprecationNotice,
                            "",
                            "Deprecated",
                            DiagnosticSeverity.Warning,
                            isEnabledByDefault: true
                        ), Location.None);
                        // context.ReportDiagnostic
                    }
                    #endregion

                    if (nodeParent != null) {
                        getNodeCaller = nodeParent;
                    }
                    
                    // Adding the field
                    string getNodeCallerToken = getNodeCaller.Length > 0 ? $"{getNodeCaller}." : "";
                    fields?.Add($"{testClass.Name} = {getNodeCallerToken}GetNode<{testClass.Type.Name}>(\"{finalGetNodePath}\");");
                }
            }
        }
        catch (Exception ex) {
            // TODO: FIXME: once one file errors, all of them error.. uhh
            string err = $"Error parsing syntax: {ex}";
            CodeLog.Add($"// {err}");
            throw new Exception(err);
        }
    }
}
