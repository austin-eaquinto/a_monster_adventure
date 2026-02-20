using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardCatchState : CharacterState
{
	public override void enter()
	{
		GD.Print("Caught");
		GetTree().Quit();
	}
}
