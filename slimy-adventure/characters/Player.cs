using Godot;
using Godot.Collections;
using System;

public partial class Player : Character
{
	public Array<Ally> followingAllies = [];

	public void addAlly(Ally ally)
	{
		if (!followingAllies.Contains(ally))
		{
			followingAllies.Add(ally);
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
		}
	}
}
