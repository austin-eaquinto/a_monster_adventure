using Godot;
using Godot.Collections;
using System;

public partial class Player : Character
{
	[Flags]
	public enum AllyAbilityFlag
	{
	    RunAbility = 1 << 1,
    	Ability2 = 1 << 2,
    	Ability3 = 1 << 3,
    	Ability4 = 1 << 4,
	}

	[Export]
	public AllyAbilityFlag allyAbilityFlags { get; set; }

	public Array<Ally> followingAllies = [];

	public void addAlly(Ally ally)
	{
		if (!followingAllies.Contains(ally))
		{
			followingAllies.Add(ally);
			GD.Print("before ",allyAbilityFlags);
			allyAbilityFlags |= (AllyAbilityFlag)(int)ally.allyAbilityType;
			GD.Print("after ",allyAbilityFlags);
		}
	}

	public int getAllyIndex(Ally ally)
	{
		return followingAllies.IndexOf(ally);
	}
	public void removeAlly(Ally ally)
	{
		if (followingAllies.Contains(ally))
		{
			followingAllies.Remove(ally);
			allyAbilityFlags &= ~(AllyAbilityFlag)(int)ally.allyAbilityType;
		}
	}
}
