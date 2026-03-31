using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	private AudioStreamPlayer _clickSound;
    public override void _Ready()
    {
		_clickSound = GetNode<AudioStreamPlayer>("UIClickSound");
        GD.Print($"¿Está el árbol pausado?: {GetTree().Paused}");
    }

	private async void _on_new_game_pressed()
	{
		if (_clickSound != null)
		{
			_clickSound.Play();
			// pause scene transition until sound is finished
			await ToSignal(GetTree().CreateTimer(0.6f), SceneTreeTimer.SignalName.Timeout);
		}
		else
		{
			GD.PrintErr("AUDIO ERROR: _clickSound is null! Check your GetNode path in _Ready().");
		}
		await Global.Instance.TransitionWorldScene("Prison",0);
	}

	private void _on_load_game_pressed()
	{
		GD.Print("Load button pressed.");
		Global.Instance.LoadGame();
	}

	private void _on_settings_pressed()
	{
		GetTree().ChangeSceneToFile("res://screens/settings/setting_screen.tscn");
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
