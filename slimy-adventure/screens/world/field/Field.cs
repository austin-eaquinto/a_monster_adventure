using Godot;
using System;

public partial class Field : Node2D
{
    private AudioStream bg_music;
    private TileMapLayer bridgesBelow;
    private TileMapLayer bridgesAbove;

    public override void _Ready()
    {
        // 1. Get the Autoload reference
        BGmusic bgmNode = GetNode<BGmusic>("/root/BGM");

        // 2. Load the audio file directly using its exact path
        bg_music = GD.Load<AudioStream>("res://audio/sounds/andorios-arcade_music4-361010.mp3");

        // 3. Send the music to the Autoload ONLY if it is currently silent
        if (bg_music != null && bgmNode != null)
        {
            if (!bgmNode.Playing)
            {
                bgmNode.PlayMusic(bg_music);
            }
        }

        // Rest of your level logic
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