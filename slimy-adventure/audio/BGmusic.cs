using Godot;
using System;

public partial class BGmusic : AudioStreamPlayer
{
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