namespace Test;
using System;
using Godot;

#pragma warning disable CS0169

public partial class TestNode : Node2D {
	[GetNode("MySprite")] private Sprite2D sprite;
	[GetNode] private CollisionShape2D collision;
}
