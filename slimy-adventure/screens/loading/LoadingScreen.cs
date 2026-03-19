using Godot;
using System;

public partial class LoadingScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("LoadingTimer").Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		// CRITICAL: Stop the timer so it doesn't fire again while the scene is changing
		GetNode<Timer>("LoadingTimer").Stop();

		var global = Global.Instance;
		if (!string.IsNullOrEmpty(global.NextScene))
		{
			GetTree().ChangeSceneToFile(global.NextScene);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
