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
		// Safety check: if the node is being removed, don't try to access the tree
		if (!IsInsideTree()) return;

		GetNode<Timer>("LoadingTimer").Stop();

		var global = Global.Instance;
		if (!string.IsNullOrEmpty(global.NextScene))
		{
			// ChangeSceneToFile is technically a deferred action anyway, 
			// but checking IsInsideTree() prevents the crash.
			GetTree().ChangeSceneToFile(global.NextScene);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
