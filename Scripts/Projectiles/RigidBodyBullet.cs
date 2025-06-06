using Godot;
using System;

public partial class RigidBodyBullet : RigidBody3D
{
	double timer = 0;
	[Export]
	public int damage = -1;
	[Export]
	public float speed = 15;
	[Export]
	public double lifetime = 15;
	[Signal]
	public delegate void DealDamageEventHandler(byte damagePoints);
	[Export]
	public PackedScene sparks;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ApplyImpulse(-GlobalTransform.Basis.Z * speed);
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
			Connect(SignalName.DealDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
			EmitSignal(SignalName.DealDamage, damage, GetGroups()[0].ToString());
		}
		if (sparks != null)
		{
			Node3D newSparks = (Node3D)sparks.Instantiate();
			newSparks.Position = Position;
			newSparks.AddToGroup(GetGroups()[0]);
			GetTree().CurrentScene.AddChild(newSparks);
		}
		QueueFree();
	}
	
}

