
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class LostAllyInstantiator : Node2D
{
    public void DoInstantiation()
    {

        foreach (int allyId in Global.instance.allyDict.Keys)
        {
            if (
                Global.instance.allyDict[allyId]["sceneName"].AsString() == Global.instance.currentSceneName &&
                !Global.instance.allyDict[allyId]["isFollowing"].AsBool() && 
                !Global.instance.allyDict[allyId]["isImprisoned"].AsBool()
                )
            {
                Ally newAlly = (GD.Load(Global.instance.allyDict[allyId]["allyScene"].AsString()) as PackedScene).Instantiate() as Ally;
                GetParent().CallDeferred("add_child",newAlly);
                newAlly.GlobalPosition = Global.instance.allyDict[allyId]["position"].AsVector2();
                newAlly.id = allyId;
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        DoInstantiation();
    }
}