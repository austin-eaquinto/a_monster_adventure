using Godot;
using System;

public partial class Ending : Control
{
	private Label finishTime;
	private Label endMessage;
	private Array<Ally> allies;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		finishTime = GetNode<Label>("VBoxContainer/Time");
		endMessage = GetNode<Label>("VBoxContainer/Message");
		allyCount = Global.player.followingAllies
		allyCount = 0;
		displayResults();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void displayResults() {
		// change the text based on how many allies escaped
		// animate how the text displays slowly
		// switch to credits scene
		
		finishTime.Text = "";
		
		if (allyCount == 3) {
			endMessage.Text = "Yay all of you escaped together!";
		}
		else if (allyCount == 2) {
			endMessage.Text = "You made it out with 2 friends";
		}
		else if (allyCount == 1) {
			endMessage.Text = "You made it out with a friend";
		}
		else {
			endMessage.Text = "One is better than none...";
		}
	}
	
	
	
	
}
