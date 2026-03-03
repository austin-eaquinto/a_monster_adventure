using Godot;
using System;

[GlobalClass]
public partial class PlayerGenericAbilityState : CharacterState
{
	[Export]
	public float cooldownTime {get; set;} = 1.0f;

	protected string inputName = "ability1";
	public Timer cooldownTimer;
	public bool timedOut = true;

    public override void _Ready()
    {
        base._Ready();
		cooldownTimer = new Timer();
		cooldownTimer.WaitTime = cooldownTime;
		cooldownTimer.OneShot = true;
		cooldownTimer.Timeout += Timeout;
		AddChild(cooldownTimer);
    }

	public void Timeout()
	{
		timedOut = true;
	}

	public override void Enter()
	{
		base.Enter();
		timedOut = false;
		cooldownTimer.Start();
		if (character != null) (character as Character).animation_name = "walk_";
	}

	public override bool EvaluateStateCondition()
    {
		return Input.IsActionJustPressed(inputName) && timedOut;
    }

}
