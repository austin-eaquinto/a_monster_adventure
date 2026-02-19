using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardChaseState : CharacterState
{
	[Export]
	public State pauseState { get; set; } = null;
	[Export]
	public State catchState { get; set; } = null;

	
	[Export]
	public Area2D captureArea { get; set;} = null;
	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;

	[Export]
	public float mustHaveBeenTheWindTime { get; set;} = 5.0f;

	protected Timer timer = null;

	public override void _Ready()
	{
		base._Ready();

		timer = new Timer();
		timer.WaitTime = mustHaveBeenTheWindTime;
		timer.Timeout += lostPlayer;
		timer.OneShot = true;
		AddChild(timer);

		captureArea.BodyEntered += BodyEnters;
	}

	protected void BodyEnters(Node2D body)
	{
		if (!body.IsInGroup("Player"))
		{
			return;
		}
		else
		{
			exit(catchState);
		}
	}

	protected void lostPlayer()
	{
		exit(pauseState);
	}

	public override void enter()
	{
		base.enter();
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			SearchForPlayer();
			
			Chase(delta);
		
	}

	protected virtual void SearchForPlayer()
	{
		bool seesPrisoner = (character as Guard).GuardSeesPrisoner((character as Guard)._targetPrisoner);
		if (!seesPrisoner) seesPrisoner = (character as Guard).GuardSeesNewTargetPrisoner();
		

		if (seesPrisoner) 
		{
			timer.Start();
		}
	}

	protected virtual void Chase(double delta)
	{
		Character prisoner = (character as Guard)._targetPrisoner;
		navAgent.TargetPosition = prisoner.GlobalPosition;
		
		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();
		(character as Guard).guardLooks(direction,delta);

		(character as Character).velocity = direction * (character as Character).Speed;


		if (Math.Abs(direction.X) >= 0.5)
		{
			if (direction.X > 0)
			{
				(character as Character).facingDirection = "right";
			} 
			else
			{
				(character as Character).facingDirection = "left";
			}
		}
		else if (direction.Y != 0.0)
		{
			if (direction.Y > 0)
			{
				(character as Character).facingDirection = "down";
			} 
			else
			{
				(character as Character).facingDirection = "up";
			}
		}
	}
}
