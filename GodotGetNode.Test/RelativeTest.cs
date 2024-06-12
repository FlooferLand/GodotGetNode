namespace Test;
using Godot;

#pragma warning disable CS0169

public partial class RelativeTestNode : Node2D {
	[GetNode("MyPlayer")] private CharacterBody2D player;
	[GetNode(nameof(player), "MySprite")] private Sprite2D sprite;
	[GetNode(nameof(player), "CollisionShape2D")] private CollisionShape2D collision;
	
	[GetNode("{player}/DeprecationTest")] private Node2D deprecationTest;
}
