using Godot;
using System;

public partial class WorldScenePortal : Area2D
{

	[Export]
	public string sceneName = "";
	[Export]
	public int playerInstantiatorId = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += onBodyEntered;
	}

	public void onBodyEntered(Node2D body)
	{
		if (body == Global.instance.player)
		{
			changeScene();
		}
	}

	public void changeScene()
	{
		Global.instance.TransitionWorldScene(sceneName,playerInstantiatorId);
	}
}
