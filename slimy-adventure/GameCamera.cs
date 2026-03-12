using Godot;
using System;

/* A global camera to better handle scene changes, screen effects, and more. */
public partial class GameCamera : Camera2D
{
    private Node2D _target;

    public override void _Ready()
    {
        // Pull persistent zoom from the Global Autoload
        var global = GetNode<Global>("/root/Global");
        Zoom = global.CurrentZoom;

        // Add this camera to a "Camera" group so the Player can find it
        AddToGroup("GameCamera");
        MakeCurrent();
    }


    public override void _Process(double delta)
    {
        if (IsInstanceValid(_target))
        {
            // Smoothly interpolate position toward the target
            GlobalPosition = GlobalPosition.Lerp(_target.GlobalPosition, (float)delta * 5.0f);
        }
    }

    // Instead of hard-coding the Player, pass in any Node2D
    public void SetTarget(Node2D newTarget)
    {
        _target = newTarget;
        GD.Print($"Camera Target set to: {newTarget.Name}. Current Zoom: {Zoom}");
    }

    public void SnapToTarget()
    {
        if (_target != null && IsInstanceValid(_target))
        {
            GlobalPosition = _target.GlobalPosition;
            // Reset any internal smoothing buffers if necessary
            ResetSmoothing();
        }
    }
}
