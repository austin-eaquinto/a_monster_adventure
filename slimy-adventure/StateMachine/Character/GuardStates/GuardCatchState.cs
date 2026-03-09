using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardCatchState : CharacterState
{
	
	[Export]
	public Area2D captureArea { get; set;} = null;

	public override bool EvaluateStateCondition()
    {
		return (character as Guard).state == Guard.GuardStates.Catch;
    }

	public override void Enter()
	{
		
		if ((character as Guard)._targetPrisoner == Global.instance.player)
		{
			Global.instance.TransitionWorldScene("Prison",0);
		}
		else if ((character as Guard)._targetPrisoner is Ally)
		{
			Ally ally = (character as Guard)._targetPrisoner as Ally;
			int allyId = ally.id;

			Global.instance.allyDict[allyId]["isImprisoned"] = true;
			Global.instance.allyDict[allyId]["isFollowing"] = false;
			ally.EmitSignal("Captured");
			ally.RemoveFromGroup("Prisoner");
			ally.QueueFree();
		}
		(character as Guard)._targetPrisoner = null;
		(character as Guard).state = Guard.GuardStates.Pause;
	}
}
