namespace Test;
using Godot;

#pragma warning disable CS0169

public partial class GenericsTestNode : GenericsTestParentNode<float> {
    [GetNode] private CollisionShape2D collision;
}
