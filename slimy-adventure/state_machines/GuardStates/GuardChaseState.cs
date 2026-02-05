using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardChaseState : CharacterState
{
	[Export]
	public State patrolState { get; set; } = null;

	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;

	protected Timer timer = null;

    public override void _Ready()
    {
        base._Ready();
		timer = new Timer();
		timer.WaitTime = 2;
		timer.Timeout += lostPlayer;
		timer.OneShot = true;
		AddChild(timer);
    }

	protected void lostPlayer()
	{
		exit(patrolState);
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
		Character player = GetTree().GetFirstNodeInGroup("Player") as Character;
		bool seesPlayer = (character as Guard).guardSeesPlayer(player);

		if (seesPlayer) 
		{
			timer.Start();
		}
	}

	protected virtual void Chase(double delta)
	{
		Character player = GetTree().GetFirstNodeInGroup("Player") as Character;
		navAgent.TargetPosition = player.GlobalPosition;
		
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
