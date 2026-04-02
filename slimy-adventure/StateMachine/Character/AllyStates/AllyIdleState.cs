using Godot;
using System;
using System.Drawing;


[GlobalClass]
public partial class AllyIdleState : CharacterState
{

	bool isPlayerClose = false;

	[Export]
	public Area2D joinArea { get; set;} = null;

	public override bool EvaluateStateCondition()
	{
		return (character as Ally).state == Ally.AllyStates.Idle;
	}

	public override void _Ready()
	{
		base._Ready();
		joinArea.BodyEntered += BodyEnters;
		joinArea.BodyExited += BodyExited;
		var dialogic = GetNode("/root/Dialogic");
		dialogic.Connect("signal_event", Callable.From<string>(OnDialogicSignal));
	}

	private void OnDialogicSignal(string argument)
	{
		if(argument == "follow_player")
		{
			var dialogic = GetNode("/root/Dialogic");
			dialogic.Disconnect("signal_event", Callable.From<string>(OnDialogicSignal));
			isPlayerClose = false;
			(character as Ally).state = Ally.AllyStates.PerfectFollow;
		}
	}

    public override void _Process(double delta)
    {
        if(isPlayerClose && Input.IsActionJustPressed("interact") && !Global.Instance.inDialogue)
		{
			run_dialogue((character as Ally).id);
		}
    }

	public void run_dialogue(int id)
	{
		if(id == 0)
		{
			var dialogic = GetNode("/root/Dialogic");
			dialogic.Call("start", "ghostHello");
		}
	}

	protected void BodyExited(Node2D body)
	{
		isPlayerClose = false;
	}

	protected void BodyEnters(Node2D body)
	{
		if (!active)
		{
			return;
		}
		if (!(body == player))
		{
			return;
		}
		else
		{
			//(character as Ally).state = Ally.AllyStates.PerfectFollow;
			isPlayerClose = true;
		}
	}

	private Player player;
	
	public override void Enter()
	{
		base.Enter();
		(character as Character).velocity = Vector2.Zero;
		player = (character as Ally).getPlayer();
		Global.Instance.Connect("AlertGuards",new Callable(this,"PrepareFlee"));
		if (character != null) (character as Character).animation_name = "idle_";

	}

    public override void Exit()
    {
        base.Exit();
		Global.Instance.Disconnect("AlertGuards",new Callable(this,"PrepareFlee"));

    }


	public void PrepareFlee(Vector2 alertPosition, Character spottedPrisoner)
	{
		GD.Print("Ally flees");
		if (spottedPrisoner == (character as Ally))
		{
			(character as Ally).state = Ally.AllyStates.Flee;
		}
	}

}
