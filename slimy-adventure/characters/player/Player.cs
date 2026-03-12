using Godot;
using Godot.Collections;
using System;

public partial class Player : Character
{

	[Export]
	public StateCondition abilitiesStateBranch {get; set;} = null;

	public Array<Ally> followingAllies = [];

    public override void _Ready()
    {
        // Find the camera in the scene tree and tell it to follow 'this'
		var camera = GetNode<GameCamera>("/root/GameCamera");

		if (camera != null)
		{
			camera.SetTarget(this);
			// Prevents the camera from "sliding" from (0,0) to the player
			camera.SnapToTarget();
		}
    }


	public void addAlly(Ally ally)
	{
		if (!followingAllies.Contains(ally))
		{
			followingAllies.Add(ally);
			if (followingAllies[followingAllies.IndexOf(ally)].abilityStateBranch != null)
				abilitiesStateBranch.AddChild(followingAllies[followingAllies.IndexOf(ally)].createNewStateBranchLink());
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
			if (followingAllies[followingAllies.IndexOf(ally)].abilityStateBranchLink != null)
				abilitiesStateBranch.RemoveChild(followingAllies[followingAllies.IndexOf(ally)].abilityStateBranchLink);
			followingAllies.Remove(ally);
		}
	}

}
