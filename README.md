# GodotGetNode

Godot Mono code generator to help you get your nodes

## Example:
```csharp
using GodotGetNode;

public partial class MyNode : Node2D {
	// Specify the name if you want to
	[GetNode("CoolSprite")] Sprite2D sprite;
    
	// Guesses the name based on the default type name ("VBoxContainer")
	[GetNode] VBoxContainer container;

	// Gets the path relative to another type!
	[GetNode("{container}/PanelContainer/Label"] Label relativeToContainer;
    
	public override void _Ready() {
		// No more GetNode<Type>("Name") over here!
	}
}
```

## Roadmap
- [ ] A complete rewrite for cleanliness sake.
- [ ] Optionally allow specifying a variable name to the left of the node path string
	- Example: `[GetNode(container, "PanelContainer/Label"] Label relativeToContainer;`
- [ ] Allow easier user debugging  &  Fix "Cannot access a disposed object"
