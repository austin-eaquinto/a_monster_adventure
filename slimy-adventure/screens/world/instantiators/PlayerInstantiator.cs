
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerInstantiator : Node2D
{
    [Export]
    public int id {get; set;} = 0;
    [Export]
    public Node2D allySpawnNode2d;

    public string playerScene = "res://characters/player/player.tscn";

    public void DoInstantiation()
    {
        Global.instance.player = (GD.Load(playerScene) as PackedScene).Instantiate() as Player;
        GetParent().CallDeferred("add_child",Global.instance.player);
        Global.instance.player.GlobalPosition = GlobalPosition;

        foreach (int allyId in Global.instance.allyDict.Keys)
        {
            if (Global.instance.allyDict[allyId]["isFollowing"].AsBool())
            {
                Ally newAlly = (GD.Load(Global.instance.allyDict[allyId]["allyScene"].AsString()) as PackedScene).Instantiate() as Ally;
                GetParent().CallDeferred("add_child",newAlly);
                newAlly.GlobalPosition = allySpawnNode2d.GlobalPosition;
                Global.instance.player.addAlly(newAlly);
                newAlly.state = Ally.AllyStates.PerfectFollow;
                newAlly.id = allyId;
            }
        }
    }
}