using Godot;
using System;

public partial class Field : Node2D
{
    [Export]
	public AudioStream bg_music { get; set; }
	private TileMapLayer bridgesBelow;
    private TileMapLayer bridgesAbove;

    public override void _Ready()
    {
        BGmusic bgmNode = GetNode<BGmusic>("/root/BGM");
		if (bg_music != null)
		{
			bgmNode.PlayMusic(bg_music);
		}
        bridgesBelow = GetNode<TileMapLayer>("BridgesBelow");
        bridgesAbove = GetNode<TileMapLayer>("BridgesAbove");

        var layerSwitches = GetTree().GetNodesInGroup("BridgeSensor");

        foreach (Node node in layerSwitches)
        {
            if (node is BridgeSensor sensor)
            {
                sensor.ChangeLayer += OnChangeLayer;
            }
        }
    }

	private void OnChangeLayer()
    {
        bridgesBelow.Enabled = !bridgesBelow.Enabled;
        bridgesAbove.Enabled = !bridgesAbove.Enabled;
    }
}
