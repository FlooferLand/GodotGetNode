using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class GetNode : Attribute {
	public string NodePath { get; private set; }
	public GetNode() : this(string.Empty) { }
	public GetNode(string nodePath) {
		NodePath = nodePath;
	}
}
