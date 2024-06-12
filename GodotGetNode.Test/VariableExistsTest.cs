namespace Test;
using Godot;

#pragma warning disable CS0169

// Should write a user-friendly error if player does not exist (if it was renamed, etc)

// ReSharper disable UnusedType.Global
public partial class VariableExistsTestNode : Node2D {
	// // [GetNode("Player")] private CharacterBody2D player;
	// [GetNode("player", "Sprite")] private Sprite2D mountainDew;
	// [GetNode("{player}/Sprite")] private Sprite2D mountainDew2;
}
