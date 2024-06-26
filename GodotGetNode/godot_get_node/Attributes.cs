﻿using System;

// Do not modify this in a user project unless you know what you're doing!
// This was generated by the GodotGetNode package.
// Deleting it will break things. Feel free to hide it.

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
	public string NodePathParent { get; private set; }
	public string NodePath { get; private set; }
	
	/// Empty constructor; Will assume the name of the node using the type name (Won't work in all cases!)
	public GetNode() : this(string.Empty) { }
	
	/// <param name="nodePath">The path to the node you wish to get (ex: <c>Player/Head/Camera</c>)</param>
	public GetNode(string nodePath) {
		NodePathParent = string.Empty;
		NodePath = nodePath;
	}
	
	/// Looks for a node relative to another node
	/// <example><code>
	///		[GetNode(nameof(container), "PanelContainer/Label"] Label relative2;
	/// </code></example>
	/// <param name="pathParent">Use <c>nameof(nodeVariable)</c> to reference another node that <paramref name="nodePath"/> will start from</param>
	/// <param name="nodePath">The path to the node you wish to get</param>
	public GetNode(string pathParent, string nodePath) {
		NodePathParent = pathParent;
		NodePath = nodePath;
	}
}
