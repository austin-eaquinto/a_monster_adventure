using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyPauseState : CharacterState
{
	[Export]
	public State nextState { get; set; } = null;

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

	public override void enter()
	{
		base.enter();
		timer.Start();
		(character as Character).velocity = Vector2.Zero;
	}
	
	public override void _Process(double delta)
	{
		return;
	}
}
