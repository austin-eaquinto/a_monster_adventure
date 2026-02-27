using Godot;
using System;

[GlobalClass]
public partial class PlayerGhostAbilityState : PlayerGenericAbilityState
{
	[Export]
	public float abilityDuration {get; set;} = 5.0f;
	public Timer durationTimer;
	public bool persists = false;

    public override void _Ready()
    {
        base._Ready();

		durationTimer = new Timer();
		durationTimer.WaitTime = abilityDuration;
		durationTimer.OneShot = true;
		durationTimer.Timeout += OnAbilityFinished;
		AddChild(durationTimer);
    }

	public void OnAbilityFinished()
	{
		if (character != null)
		{
			var c = character as Character;
			var sprite = c.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			sprite.Modulate = new Color(1,1,1,1f);
		}

		persists = false;

	}

	public override void Enter()
	{
		base.Enter();
		persists = true;
		durationTimer.Start();

		if (character != null)
		{
			var c = character as Character;
			var sprite = c.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			sprite.Modulate = new Color(1,1,1,0.3f);
		}
	}

	public override bool EvaluateStateCondition()
    {
		return base.EvaluateStateCondition() || persists;
    }

}
