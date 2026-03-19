using Godot;
using System;

public partial class Dialogue : CanvasLayer
{
	private double CHAR_READ_RATE = 0.02;
	private MarginContainer textboxContainer;
	private Label startSymbol;
	private Label endSymbol;
	private Label label;
	
	private enum State { READY, READING, FINISHED };
	private State currentState = State.READY;
	
	public override void _Ready() {
		textboxContainer = GetNode<MarginContainer>("TextboxContainer");
		startSymbol = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/Start");
		endSymbol = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/End");
		label = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/Label");
		
		GD.Print("Starting State: State.READY");
		
		HideTextbox();
		AddText("This text is going to be added!");
	}

	public override void _Process(double delta) {
		switch (currentState) {
			case State.READY: 
				break;
			case State.READING:
				break;
			case State.FINISHED:
				if (Input.IsActionPressed("ui_accept")) 
				{
					ChangeState(State.READY);
					HideTextbox();
				}
				break;
					
		}
	}
	
	public void HideTextbox() {
		startSymbol.Text = "";
		endSymbol.Text = "";
		label.Text = "";
		textboxContainer.Hide();
	}
	
	public void ShowTextbox() {
		startSymbol.Text = "*";
		textboxContainer.Show();
	}
	
	public void AddText(string nextText) {
		label.Text = nextText;
		ChangeState(State.READING);
		ShowTextbox();
		
		var tween = CreateTween();
		tween.TweenProperty(label, "visible_ratio", 1.0f, nextText.Length * CHAR_READ_RATE).From(0.0f);
		tween.Finished += OnTweenFinished;
	}
	
	private void OnTweenFinished(){
		endSymbol.Text = "v";
		ChangeState(State.FINISHED);
	}
	
	private void ChangeState(State nextState) {
		currentState = nextState;
		switch (currentState) {
			case State.READY: 
				GD.Print("Changing to: State.READY");
				break;
			case State.READING:
				GD.Print("Changing to: State.READING");
				break;
			case State.FINISHED:
				GD.Print("Changing to: State.FINISHED");
				break;
		}
	}
}
