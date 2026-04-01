using Godot;

[GlobalClass]
public partial class CameraLimitMarker : Node2D
{
	public enum CameraLimitType
	{
		TopLeft = 0,
		BottomRight = 1
	}

	[Export]
	public CameraLimitType limitType {get; set;} = CameraLimitType.TopLeft;

	public override void _Ready()
	{
		base._Ready();
		GD.Print($"[CameraLimitMarker._Ready] Node: {Name}, limitType: {limitType}, Position: {GlobalPosition}");
		Global.Instance.setCameraLimits(GlobalPosition,(int)limitType);
		var (topLeft, bottomRight) = Global.Instance.getCameraLimits();
		GD.Print($"[CameraLimitMarker._Ready] After setting limits - TopLeft: {topLeft}, BottomRight: {bottomRight}");
	}
}
