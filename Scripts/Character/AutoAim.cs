using Godot;
using System;

public partial class AutoAim : Node3D
{

	[Export]
	bool enabled = true;
	Transform3D lookat;
	public Node3D target;
	[Export]//in case we need to offset aim (for grenades or headshots, for example) this might also be useful for interpolation / imperfect tracking
	public float hOffset, vOffset, trackingLatency = 0;
	
	[Export] //clamp values for horizontal and vertical rotation (IN DEGREES)
	public float hMin = -Mathf.Inf, hMax = Mathf.Inf, vMin = -Mathf.Inf, vMax = Mathf.Inf;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		target = GetTree().Root.GetNode<Node3D>("Node3D/Player"); //at some point this should change to either simply face foward or target a player depending on the situation.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!enabled) return;
		Vector3 targetPos = new Vector3(target.Position.X + hOffset, target.Position.Y + vOffset, target.Position.Z); //setting the position for the targetAim to look at
		/* lookat.origin = Transform3D.origin;
		lookat = lookat.LookingAt(targetPos, Vector3.Up);
		Transform3D.InterpolateWith(lookat, 1);*/

		
		if (targetPos.Normalized() != Vector3.Up && targetPos != GlobalPosition)
		{
			//TODO: implement tracking latency.
			LookAt(targetPos, Vector3.Up);
		}
			
		Vector3 finalRot = Rotation;   
		finalRot.X = Mathf.Clamp(Rotation.X, Mathf.DegToRad(vMin), Mathf.DegToRad(vMax)); //we want to clamp this first
		finalRot.Y = Mathf.Clamp(Rotation.Y, Mathf.DegToRad(hMin), Mathf.DegToRad(hMax));
		
		Rotation = finalRot;
	}
}
