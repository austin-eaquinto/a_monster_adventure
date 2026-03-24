using Godot;
using System;
using System.Threading.Tasks;

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

	public async void onBodyEntered(Node2D body)
{
    // body is Player works if the script is [GlobalClass]
    // body.IsInGroup("Player") works if you added the player to that group
    if (body is Player || body.IsInGroup("Player"))
    {
        await changeScene();
    }
}

	public async Task changeScene()
	{
		await Global.Instance.TransitionWorldScene(sceneName,playerInstantiatorId);
	}
}
