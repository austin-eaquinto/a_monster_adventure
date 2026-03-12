using Godot;
using System;
using System.Drawing;
using System.Threading.Tasks;

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
		HandleCatch();
	}

	private async void HandleCatch()
	{
		if ((character as Guard)._targetPrisoner == Global.Instance.player)
		{
			await Global.Instance.TransitionWorldScene("Prison",0);
		}
		else if ((character as Guard)._targetPrisoner is Ally)
		{
			Ally ally = (character as Guard)._targetPrisoner as Ally;
			int allyId = ally.id;

			Global.Instance.allyDict[allyId]["isImprisoned"] = true;
			Global.Instance.allyDict[allyId]["isFollowing"] = false;
			ally.EmitSignal("Captured");
			ally.RemoveFromGroup("Prisoner");
			ally.QueueFree();
		}
		(character as Guard)._targetPrisoner = null;
		(character as Guard).state = Guard.GuardStates.Pause;
	}
}
