using Godot;
using System;

public partial class Guard : Character
{
	[Export]
	public Line2D line2D { get; set; } = null;

	[Export]
	public RayCast2D raycast2D { get; set; } = null;
	
	[Export]
	public Node2D lightPivot { get; set; } = null;

	[Export]
	public float visionRadius { get; set; } = 600.0f;
	
	[Export]
	public float visionArc { get; set; } = (float)Math.PI / 2.0f;
	
	Vector2 lookDirection = Vector2.Zero;

	[Export]
	float lookSpeed { get; set; } = 5.0f;

	public void guardLooks(Vector2 targetLookDirection, double delta)
	{
		float newAngle = Mathf.RotateToward(lookDirection.Angle(), targetLookDirection.Angle(), lookSpeed * (float)delta);
		lookDirection = Vector2.FromAngle(newAngle);

		lightPivot.Rotation = newAngle - Mathf.Pi / 2.0f;
	}

	public bool guardSeesPlayer(Character player)
	{
		Vector2 directionToPlayer = ToLocal(player.GlobalPosition).Normalized();

		if (Math.Abs(lookDirection.AngleTo(directionToPlayer)) > visionArc)
		{
			return false;
		}

		raycast2D.TargetPosition = directionToPlayer * visionRadius;
		raycast2D.ForceRaycastUpdate();

		var collider = raycast2D.GetCollider();

		if (collider != player)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

}
