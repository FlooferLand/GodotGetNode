# GodotGetNode
![GitHub Issues](https://img.shields.io/github/issues/FlooferLand/GodotGetNode?link=https%3A%2F%2Fgithub.com%2FFlooferLand%2FGodotGetNode%2Fissues)

A production-ready Godot 4.x source generator to help you get your nodes!

## Example
```csharp
using Godot;

public partial class MyNode : Node2D {
	// Specify the name of the node
	[GetNode("Player")] CharacterBody2D player;

	// Custom syntax to get a node relative to another node!
	[GetNode(nameof(player), "Sprite")] Sprite2D sprite;
	// .. or a neater, but type unsafe syntax:
	[GetNode("{player}/Sprite")] Sprite2D sprite;
    
	public override void _Ready() {
		// No more GetNode<Sprite2D>("Player/Sprite") over here!
	}
}
```

## Note
This repo might seem inactive, but this project works just fine.

I don't update this repo often as it doesn't need any updates, but I've been using it myself in my own Godot projects for about a year now as of the time I'm writing this.

If you do encounter any issues, please [open up an issue or a PR on the repository!](https://github.com/FlooferLand/GodotGetNode/issues)

![Hide inlays in your IDE for the best experience](https://raw.githubusercontent.com/FlooferLand/GodotGetNode/main/hideInlays.png)

## Compatibility
_Actively tested for Godot 4.x_

| Version | Godot 3.x | Godot 4.x |
|---------|-----------|-----------|
| 0.1.x   | Likely    | ✔         |
| 0.2.x   | Untested  | ✔         |

All this project does is generate some `GetNode<>()`s on `TreeEntered` using types and names extracted from your code,
so theoretically it should work on Godot 3.x

The .NET version and features the project is using might block use on 3.x though.
If you use 3.x and this project doesn't work, please [open up an issue](https://github.com/FlooferLand/GodotGetNode/issues) and I will fix it right away!

## Roadmap
- [x] Optionally allow specifying a variable name to the left of the node path string
	- Example: `[GetNode(container, "PanelContainer/Label"] Label relativeToContainer;`
    - -- There's no neat way of doing this, because of C# its required to use `nameof(..)`
- [ ] A complete rewrite for cleanliness' sake. 
    - [ ] Allow easier user debugging  &  Fix "Cannot access a disposed object" in some rare cases
