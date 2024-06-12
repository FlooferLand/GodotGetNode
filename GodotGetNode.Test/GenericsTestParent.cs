namespace Test;
using Godot;

#pragma warning disable CS0169

// ReSharper disable UnusedType.Global
public partial class GenericsTestParentNode<TGeneric> : Node2D {
	[GetNode("MySprite")] private Sprite2D sprite;
	private TGeneric parentValue;
}
