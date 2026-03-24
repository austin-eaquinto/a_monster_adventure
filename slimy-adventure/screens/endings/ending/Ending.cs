using Godot;
using System;

public partial class Ending : Control
{
	private Label finishTime;
	private Label endMessage;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		finishTime = GetNode<Label>("VBoxContainer/Time");
		endMessage = GetNode<Label>("VBoxContainer/Message");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void displayResults(int allyCount) {
		finishTime.Text = "";
		endMessage.Text = "";
		
	}
	
	
	
	
}
