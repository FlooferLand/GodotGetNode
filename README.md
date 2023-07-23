# GodotGetNode
#### WARNING! Package is still a work in progress!

---

Godot Mono code generator to help you get your nodes

## Example:
```csharp
using GodotGetNode;

public partial class MyNode : Node2D {
	// Specify the name if you want to
	[GetNode("CoolSprite")] Sprite2D sprite;
    
	// Guesses the name based on the default type name ("CollisionShape2D")
	[GetNode] CollisionShape2D collision;
    
	public override void _Ready() {
		// No more GetNode<Type>("Name") over here!
	}
}
```