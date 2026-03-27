using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	private async void _on_new_game_pressed()
	{
		// Old merge
		// Global.Instance.NextScene = "res://screens/world/prison/prison.tscn";
		// await Global.Instance.TransitionScene("LoadingScreen");
		// await Global.Instance.TransitionWorldScene("Prison", 0);

		// new merge
		Global.Instance.NextScene = "res://screens/world/prison/prison.tscn";
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
