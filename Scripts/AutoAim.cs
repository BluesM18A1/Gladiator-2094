using Godot;
using System;

public partial class AutoAim : Node3D
{
	Transform3D lookat;
	public Node3D target;
	[Export]//in case we need to offset aim (for grenades or headshots, for example) this might also be useful for interpolation / imperfect tracking
	public float hOffset, vOffset, trackingLatency;
	
	[Export] //clamp values for horizontal and vertical rotation (IN DEGREES)
	public float hMin = -Mathf.Inf, hMax = Mathf.Inf, vMin = -Mathf.Inf, vMax = Mathf.Inf;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		target = GetNode<Node3D>(GetTree().Root.GetNode("Node3D/Player").GetPath()); //I should definitely think of a way to make this modular without sacrificing the ability to easily target the player.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		//this is a somewhat rarely used feature in C# thats super useful to remember. Basically goes "bool ? true condition : false condition";
		//float xVal = hAim ? target.Position.X + hOffset : Position.X; 
		//float yVal = vAim ? target.Position.Y + vOffset : Position.Y;
		//wait a sec if we're using clamps then we don't even need to have enable flags! ahwell. I'm keeping these commented because they are a good piece of knowledge to reference.

		Vector3 targetPos = new Vector3(target.Position.X + hOffset, target.Position.Y + vOffset, target.Position.Z); //setting the position for the targetAim to look at
		/* lookat.origin = Transform3D.origin;
		lookat = lookat.LookingAt(targetPos, Vector3.Up);
		Transform3D.InterpolateWith(lookat, 1);*/

		//welp, I can't figure out aim interpolation for the life of me. I suppose having all the bullets as projectiles instead of hitscan makes up for the perfect aim.
		LookAt(targetPos, Vector3.Up);
		Vector3 finalRot = Rotation;   
		finalRot.X = Mathf.Clamp(Rotation.X, Mathf.DegToRad(vMin), Mathf.DegToRad(vMax)); //we want to clamp this first
		finalRot.Y = Mathf.Clamp(Rotation.Y, Mathf.DegToRad(hMin), Mathf.DegToRad(hMax));
		
		Rotation = finalRot;
	}
}
