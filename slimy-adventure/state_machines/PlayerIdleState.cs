using Godot;
using System;

[GlobalClass]
public partial class PlayerIdleState : State
{
	[Export]
	public State playerWalkState { get; set; } = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}
	
	public override void enter()
	{
		enteredParent = GetParent();
		GD.Print(enteredParent);
		SetProcess(true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
