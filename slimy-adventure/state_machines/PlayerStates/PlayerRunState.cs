using Godot;
using System;

[GlobalClass]
public partial class PlayerRunState : CharacterState
{
	[Export]
	public State playerIdleState { get; set; } = null;
	[Export]
	public State playerWalkState { get; set; } = null;

	public override void enter()
	{
		GD.Print("Run");
		base.enter();
		if (character != null) (character as Character).animation_name = "run_";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");

		if (input != Vector2.Zero)
		{
			(character as Character).velocity = input * (character as Character).Speed * 1.5f;

			if (input.X != 0.0)
			{
				if (input.X > 0)
				{
					(character as Character).facingDirection = "right";
				} 
				else
				{
					(character as Character).facingDirection = "left";
				}
			}
			else if (input.Y != 0.0)
			{
				if (input.Y > 0)
				{
					(character as Character).facingDirection = "down";
				} 
				else
				{
					(character as Character).facingDirection = "up";
				}
			}
			
			if (!((character as Player).allyAbilityFlags == Player.AllyAbilityFlag.RunAbility && Input.IsActionPressed("run")))
			{
				exit(playerWalkState);
			}
		}
		else
		{
			(character as Character).velocity = Vector2.Zero;
			exit(playerIdleState);
		}
	}
}
