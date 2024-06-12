using System;

// Do not modify this in a user project unless you know what you're doing!

/// <summary>
/// Runs <c>Node.GetNode()</c> for you!
///
/// <h3>Examples:</h3>
/// <code>
///	    [GetNode("MyCollision")] private CollisionShape2D collision;
/// </code>
/// 
/// Additionally, you can use the following syntax to call <c>GetNode</c> on another node:
/// <code>
///     [GetNode] private Sprite2D someSprite;
///	    [GetNode("{someSprite}/MyCollision")] private CollisionShape2D collision;
///     .. or ..
///	    [GetNode(nameof(someSprite), "MyCollision")] private CollisionShape2D collision;
/// </code>
/// 
/// <h3>Optional parameters:</h3>
/// - <c>pathParent</c>: The node to start from
/// - <c>nodePath</c>: The path for the node. If this is empty it automatically guesses the path based on the type name
/// <br/>
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class GetNode : Attribute {
	public string? NodePathParent { get; private set; }
	public string NodePath { get; private set; }
	
	// Empty constructor
	public GetNode() : this(string.Empty) { }
	
	// Node path constructor
	public GetNode(string nodePath) {
		NodePath = nodePath;
	}
	
	// Starting relative to a node
	public GetNode(string pathParent, string nodePath) {
		NodePathParent = pathParent;
		NodePath = nodePath;
	}
}
