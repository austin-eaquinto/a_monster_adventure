using Godot;
using System;

public partial class BGmusic : AudioStreamPlayer
{
    public void PlayMusic(AudioStream musicStream)
    {
        // check if the specific song is already playing
        if (Stream == musicStream)
        {
            return;
        }

        // if it's a new song, assign it and play
        Stream = musicStream;
        Play();
    }
}
