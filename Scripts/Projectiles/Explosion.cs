using Godot;
using System;

public partial class Explosion : Area3D
{
	double timer = 0;
	[Export]
	public int damage = -1;
	[Export]
	public float lifetime = 15;
	[Export]
	public PackedScene sparks;

	[Signal]
	public delegate void DealExplosiveDamageEventHandler(byte damagePoints);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (sparks != null)
		{
			Node3D newSparks = (Node3D)sparks.Instantiate();
			newSparks.Position = Position;
			GetTree().CurrentScene.AddChild(newSparks);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer+= delta;
		if (timer > lifetime) QueueFree();
	}
	private void _OnCollisionEnter(Node body)
	{
		if (body.HasMethod("UpdateHealth"))
		{
			Node3D target = (Node3D)body;
			Vector3 diff = target.Position - Position;
			float f = Mathf.Sqrt((diff.X * diff.X) + (diff.Y * diff.Y) + (diff.Z * diff.Z));
			f = f - 1; //adding 1 unit of tolerance for max damage
			f = Mathf.Clamp(f,1,5); //making sure the damage reduction doesnt go in negatives or too low
			Connect(SignalName.DealExplosiveDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
			EmitSignal(SignalName.DealExplosiveDamage, damage / f, GetGroups()[0].ToString());
			//GD.Print(damage / f);
		}
		
	}
}
