using System;

// Do not modify this in a user project unless you know what you're doing!

/// <summary>
/// <h3>Optional parameters:</h3>
/// - <c>nodePath</c>: The path for the node. If this is empty it automatically guesses the path based on the type name
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class GetNode : Attribute {
	public string NodePath { get; private set; }
	public GetNode() : this(string.Empty) { }
	public GetNode(string nodePath) {
		NodePath = nodePath;
	}
}
