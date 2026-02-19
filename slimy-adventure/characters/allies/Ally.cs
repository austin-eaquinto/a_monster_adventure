using Godot;
using System;

[GlobalClass]
public partial class Ally : Character
{

	[Export]
    public Player player {get; set;} = null;

}
