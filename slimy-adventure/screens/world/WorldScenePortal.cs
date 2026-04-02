using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
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
		if(body is Ally) return;

		if (body is Player || body.IsInGroup("Player"))
		{
			// Call the non-async wrapper instead
			CallDeferred(nameof(TriggerSceneChange));
		}
	}

	// Godot can "see" this method easily
	private void TriggerSceneChange()
	{
		// Fire and forget the Task
		_ = changeScene();
	}

	public async Task changeScene()
	{
		await Global.Instance.TransitionWorldScene(sceneName, playerInstantiatorId);
	}
}
