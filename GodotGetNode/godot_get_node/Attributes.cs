using System;

// Do not modify this in a user project unless you know what you're doing!

/// <summary>
/// <p>Runs <c>Node.GetNode()</c> for you!</p>
/// <p>Additionally, you can use the following syntax to call <c>GetNode</c> on another node:
/// <code>
/// [GetNode] private Sprite2D someNode;
///	[GetNode("{someNode}/MyCollision")] private CollisionShape2D collision;
/// </code></p>
/// 
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
