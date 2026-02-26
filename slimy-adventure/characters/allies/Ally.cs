using Godot;
using System;

[GlobalClass]
public partial class Ally : Character
{

	[Export]
	public Player player {get; set;} = null;
	[Export]
	public RayCast2D raycast2D {get; set;} = null;
	
	public enum AllyAbilityType
	{
		RunAbility = 1 << 1,
		Ability2 = 1 << 2,
		Ability3 = 1 << 3,
		Ability4 = 1 << 4,
	}
	
	[Export]
	public AllyAbilityType allyAbilityType {get; set;} = AllyAbilityType.RunAbility;
	
	public enum AllyStates
	{
		Idle = 1 << 1,
		Follow = 1 << 2,
		Flee = 1 << 3,
	}
	
	[Export]
	public AllyStates state {get; set;} = AllyStates.Idle;
}
