namespace Test;
using Godot;

#pragma warning disable CS0169

public partial class BasicTestNode : Node2D {
	[GetNode("MySprite")] private Sprite2D sprite;
	[GetNode] private CollisionShape2D collision;
}
