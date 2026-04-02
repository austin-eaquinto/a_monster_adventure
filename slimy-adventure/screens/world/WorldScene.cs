using Godot;
using System;

public partial class WorldScene : Node2D
{
	[Export]
	string sceneName = "Field";
	private AudioStream bg_music;

	// Called when the node enters the scene tree for the first time.
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
		// used to correct scene name
		Global.Instance.currentSceneName = sceneName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
