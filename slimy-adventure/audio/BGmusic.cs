using Godot;
using System;

public partial class BGmusic : AudioStreamPlayer
{
    public override void _Ready()
    {
        // Set the volume to -15 decibels. 
        // Remember to use the 'f' suffix since VolumeDb expects a float.
        this.VolumeDb = -10.0f;

        // Since you bypass the Inspector by using a .cs Autoload, 
        // you also have to manually enable Autoplay if you want it to start immediately.
        this.Autoplay = true;
    }
    public void PlayMusic(AudioStream musicStream)
    {
        if (musicStream == null) return;

        if (Stream != null && Stream.ResourcePath == musicStream.ResourcePath)
        {
            return;
        }

        Stream = musicStream;
        Play();
    }

    // Add this new method!
    public void StopMusic()
    {
        Stop(); // Godot's built-in command to stop the audio
        Stream = null; // Erase the current song from memory so it doesn't block future plays
    }
}