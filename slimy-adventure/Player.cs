using Godot;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 300;

	public Vector2 ScreenSize;
	
	private string _lastDirection = "down";

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		velocity.X = Input.GetAxis("move_left", "move_right");
		velocity.Y = Input.GetAxis("move_up", "move_down");

		// Horizontal movement has priority over Vertical
		if (velocity.X != 0)
		{
			velocity.Y = 0;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			// Casting Speed to (float) to prevent math errors
			velocity = velocity.Normalized() * (float)Speed;

			/* ---Movement Animation Below--- */
			if (velocity.X != 0)
			{
				if (velocity.X > 0)
				{
					animatedSprite2D.Animation = "walk_right";
					_lastDirection = "right";
				}
				else
				{
					animatedSprite2D.Animation = "walk_left";
					_lastDirection = "left";
				}
			}
			else if (velocity.Y != 0)
			{
				if (velocity.Y > 0)
				{
					animatedSprite2D.Animation = "walk_down";
					_lastDirection = "down";
				}
				else
				{
					animatedSprite2D.Animation = "walk_up";
					_lastDirection = "up";
				}
			}
			/* ---Movement Animation Above--- */
		}
		else
		{
			animatedSprite2D.Play("idle_" + _lastDirection);
		}

		Position += velocity * (float)delta;

		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}
