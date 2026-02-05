using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 300;
	
	public String facingDirection = "down";
	public String animation_name = "idle_";

	public Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
<<<<<<< HEAD:slimy-adventure/Player.cs
=======
		Velocity = velocity;
		MoveAndSlide();
		velocity = Velocity;
>>>>>>> eb8ac6dbd45f770989ee99a30ef40372d3fdca40:slimy-adventure/Character.cs
		
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play(animation_name + facingDirection);

		MoveAndSlide();
	}
<<<<<<< HEAD:slimy-adventure/Player.cs
}
=======
}
>>>>>>> eb8ac6dbd45f770989ee99a30ef40372d3fdca40:slimy-adventure/Character.cs
