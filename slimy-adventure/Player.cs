using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 300;
	
	public String facingDirection = "down";
	public String animation_name = "idle_";

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		MoveAndSlide();
		
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play(animation_name + facingDirection);
		
	}
