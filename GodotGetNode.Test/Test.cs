namespace Test;
using System;
using Godot;

public partial class TestNode : Node2D {
	[GetNode("MySprite")] private Sprite2D sprite;
	[GetNode] private CollisionShape2D collision;
}
