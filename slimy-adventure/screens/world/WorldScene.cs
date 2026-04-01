using Godot;
using System;

public partial class WorldScene : Node2D
{
	[Export]
	string sceneName = "Field";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// used to correct scene name
		Global.Instance.currentSceneName = sceneName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
