using Godot;
using System;

public partial class Prison : Node2D
{
    // Removed the [Export] and { get; set; } so it matches Field.cs perfectly
    private AudioStream bg_music;

    private bool _isPlayerNearby = false;
    public string NextScenePath = "res://screens/world/field/field.tscn";
    
    public override void _Ready()
    {
        GD.Print("--- PRISON READY START ---");

        // Using GetNodeOrNull prevents the silent crash!
        BGmusic bgmNode = GetNodeOrNull<BGmusic>("/root/BGM");
        
        if (bgmNode == null)
        {
            GD.PrintErr("ERROR 1: Failed to find /root/BGM, or the node is NOT an AudioStreamPlayer!");
        }
        else
        {
            GD.Print("SUCCESS 1: Found the Autoload BGM node!");
        }

        // Attempt to load the audio file
        bg_music = GD.Load<AudioStream>("res://audio/sounds/andorios-arcade_music4-361010.mp3");
        
        if (bg_music == null)
        {
            GD.PrintErr("ERROR 2: Could not load the audio file from that path!");
        }
        else
        {
            GD.Print("SUCCESS 2: Audio file loaded into memory!");
        }

        // If both succeeded, play the music
        if (bg_music != null && bgmNode != null)
        {
            GD.Print("SUCCESS 3: Sending track to Autoload...");
            bgmNode.PlayMusic(bg_music);
        }

        GD.Print("--- PRISON READY END ---");
        
        // Reconnect your portal signals
        // BodyEntered += OnBodyEntered;
        // BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        if(_isPlayerNearby && Input.IsActionJustPressed("interact"))
        {
            Escape();
        }
    }

    private async void Escape()
    {
        await Global.Instance.TransitionScene("Field"); 
        _isPlayerNearby = false;
        GD.Print("Slime is trying to escape");
    }

    public void OnBodyEntered(Node2D body)
    {
        GD.Print(body.Name);
        if(body.Name == "Player")
        {
            _isPlayerNearby = true;
            GD.Print("Press Space Bar to scape");
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if(body.Name == "Player")
        {
            _isPlayerNearby = false;
            GD.Print("Player has left the toilet");
        }
    }
}