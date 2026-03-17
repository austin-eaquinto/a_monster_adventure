using Godot;
using System;

// Text file: "res://screens/textbox/text.json"

public partial class Textbox : Control
{
	List dialogue = new List<>();
	int current_dialogue_index = 0;
	enum States 
	{
		IDLE, 
		STARTING, 
		WORLD, 
		ENDING
	};
	States state = States.IDLE;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	private void set_state(int new_state)
	{
		States previous_state = state;
		state = new_state;
	}
	
	private void load_text()
	{
		string path = "res://screens/textbox/text.json";

		// In Godot C#, you'd typically still use FileAccess
		var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);

		{
			string content = file.GetAsText();
			file.Close();
	}
	}
	
}
