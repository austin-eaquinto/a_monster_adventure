using Godot;
using System;

public partial class MainMenu : Control
{
    private void _on_new_game_pressed()
    {
        
    }

    private void _on_load_game_pressed()
    {
        
    }

    private void _on_settings_pressed()
    {
        
    }

    private void _on_credits_pressed()
    {
        GetTree().ChangeSceneToFile("res://screens/credits/credits.tscn");
    }

    private void _on_quit_pressed()
    {
        GetTree().Quit();
    }
}
