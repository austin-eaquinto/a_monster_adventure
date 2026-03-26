using Godot;
using System;
using Godot.Collections;

public partial class Ending : Control
{
    private Label finishTime;
    private Label endMessage;
    private Label congratsMessage;
    private Array<Ally> allies = [];
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        finishTime = GetNode<Label>("VBoxContainer/Time");
        endMessage = GetNode<Label>("VBoxContainer/Message");
        congratsMessage = GetNode<Label>("VBoxContainer/Label");
        // allies = Global.Instance.player.followingAllies;
        displayAllies();
        displayResults();
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    
    public void displayResults() {
        // change the text based on how many allies escaped
        // switch to credits scene
        
        finishTime.Text = "Time: Placeholder";
        finishTime.VisibleRatio = 0.0f;
        
        if (allies.Count == 3) {
            endMessage.Text = "Well done, You are strongest together!";
        }
        else if (allies.Count == 2) {
            endMessage.Text = "Now we know who your least favorite was...";
        }
        else if (allies.Count == 1) {
            endMessage.Text = "I guess one is better than none...";
        }
        else {
            endMessage.Text = "Wow... so much for teamwork...";
        }
        endMessage.VisibleRatio = 0.0f;
        
        var tween = CreateTween();
        var charReadRate = 0.1f;

        tween.TweenProperty(congratsMessage, "visible_ratio", 1.0f, congratsMessage.Text.Length * charReadRate).From(0.0f);
        tween.TweenInterval(1.0f); 
        tween.TweenProperty(endMessage, "visible_ratio", 1.0f, endMessage.Text.Length * charReadRate);
        tween.TweenInterval(1.0f); 
        tween.TweenProperty(finishTime, "visible_ratio", 1.0f, finishTime.Text.Length * charReadRate);
    }
    
    public void displayAllies() {
        foreach (Ally ally in allies) {
            AnimatedSprite2D allySprite = ally.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            GD.Print(allySprite);
        }
    }
    
}
