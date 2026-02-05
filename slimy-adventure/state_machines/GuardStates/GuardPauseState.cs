using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardPauseState : CharacterState
{
	[Export]
	public State nextState { get; set; } = null;
	[Export]
	public State chasePlayerState { get; set; } = null;

	[Export]
	public float pauseTimer { get; set; } = 2.0f;

	protected Timer timer = null;

	public override void _Ready()
	{
		base._Ready();
		timer = new Timer();
		timer.WaitTime = pauseTimer;
		timer.Timeout += unpause;
		timer.OneShot = true;
		AddChild(timer);
	}

	protected void unpause()
	{
		exit(nextState);
	}

	protected virtual void SearchForPlayer()
	{
		Character player = GetTree().GetFirstNodeInGroup("Player") as Character;
		bool seesPlayer = (character as Guard).guardSeesPlayer(player);

		if (seesPlayer) 
		{
			timer.Stop();
			exit(chasePlayerState);
		}
	}

	public override void enter()
	{
		base.enter();
		timer.Start();
		(character as Character).velocity = Vector2.Zero;
	}
	
	public override void _Process(double delta)
	{
		SearchForPlayer();
	}
}
