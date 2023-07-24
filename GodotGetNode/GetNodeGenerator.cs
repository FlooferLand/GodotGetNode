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
public class GetNodeGenerator : ISourceGenerator {
    public void Execute(GeneratorExecutionContext context) {
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            return;
        
        // Generating the code
        foreach (var getNodes in receiver.WorkItems) {
            var result = new StringBuilder();
            var names = getNodes.Key.Split('.');
            var spaceName = names[0];
            var className = names[1];
            result.AppendLine($"namespace {spaceName};");
            result.AppendLine("using Godot;");
            result.AppendLine("using System;");
            
            #region The Weird-Looking Code I'll Never Touch Again
            result.AppendLine($$"""

                                partial class {{className}} {
                                    public {{className}}() {
                                        TreeEntered += () => {
                                """);
            getNodes.Value.ToList().ForEach(v => result.AppendLine($"\t\t {v.Trim()}"));
            result.AppendLine("""
                              
                                      };
                                  }
                              }
                              """);
            #endregion
            
            // Saving the code + 
            var parsed = ($"\n{string.Join("\n", receiver.Log)}\n" + result).Trim();
            if (Debugger.IsAttached) {
                var parsedLc = $"---- {className}.cs ----\n";
                var i = 0;
                foreach (var line in parsed.Split('\n')) {
                    parsedLc += $"[{i}] {line}\n";
                    i += 1;
                }
                parsedLc += "\n\n";
            
                // [Put a break-point here in case of emergency!]
                Console.WriteLine(parsedLc);
            }
            context.AddSource($"{className}.cs", SourceText.From(parsed, Encoding.UTF8));
        }
    }

    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForPostInitialization(_ => {
            /*ctx.AddSource("GetNode",
                @"
                    using System;

                    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
                    public class GetNode : Attribute {
                        public string NodePath { get; private set; }
                        public GetNode() : this(string.Empty) { }
                        public GetNode(string nodePath) {
                            NodePath = nodePath;
                        }
                    }".Trim());*/
        });
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
}

public class SyntaxReceiver : ISyntaxContextReceiver {
    public List<string> Log { get; } = new();
    public readonly Dictionary<string, List<string>> WorkItems = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        try {
            if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax) {
                foreach (var variableName in fieldDeclarationSyntax.Declaration.Variables) {
                    var testClass = (IFieldSymbol)context.SemanticModel.GetDeclaredSymbol(variableName)!;
                    var attribute = testClass.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == "GetNode");
                    var getNodeCaller = "";
                    if (attribute != null) {
                        var type = testClass.ContainingType.ToString();
                        if (!WorkItems.TryGetValue(type, out var fields)) {
                            fields = new List<string>();
                            WorkItems.Add(type, fields);
                        }

                        // Parsing special referring syntax
                        string input = (attribute.ConstructorArguments.Length == 0)
                            ? testClass.Type.Name
                            : $"{attribute.ConstructorArguments.First().Value}";
                        string result = input;
                        if (input.Contains("{") && input.Contains("}")) {
                            // Getting the referenced variable's name
                            string referencedVar = "";
                            bool insideBlock = false;
                            foreach (char c in input) {
                                if (insideBlock && !"{}".Contains(c)) {
                                    referencedVar += c;
                                }
                                
                                if (c == '{')
                                    insideBlock = true;
                                else if (c == '}')
                                    insideBlock = false;
                            }
                            
                            // TODO: Checking if the variable exists
                            // var parent = context.Node.Parent;
                            
                            // Removing the reference from the input
                            result = input.Replace($"{{{referencedVar}}}/", "");
                            
                            // GetNode-ing it
                            getNodeCaller = $"{referencedVar}.";
                        }
                        
                        // Adding the field
                        fields.Add($"{testClass.Name} = {getNodeCaller}GetNode<{testClass.Type.Name}>(\"{result}\");");
                    }
                }
            }
        }
        catch (Exception ex) {
            Log.Add($"Error parsing syntax: {ex}");
        }
    }
}
