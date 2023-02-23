using Godot;
using System;

public partial class Bullet : Area3D
{
	double timer = 0;
	[Export]
	public int damage = -1;
	[Export]
	public float speed = 15;
	[Export]
	public double lifetime = 15;
	[Signal]
	public delegate void DealDamageEventHandler(int damagePoints);
	[Export]
	public PackedScene sparks;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		timer+= delta;
		if (timer > lifetime) QueueFree();
	}
	public override void _PhysicsProcess(double delta)
	{
		Position -= GlobalTransform.Basis.Z * (float)(speed * delta);
	}
	private void _OnCollisionEnter(Node body)
	{
		
		if (body.HasMethod("UpdateHealth"))
		{
			//body.Call("UpdateHealth", damage);
			Connect(SignalName.DealDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
			EmitSignal(SignalName.DealDamage, damage);
		}

		if (sparks != null)
		{
			Node3D newSparks = (Node3D)sparks.Instantiate();
			newSparks.Position = Position + GlobalTransform.Basis.Z * 0.4f; //the offset is so we can see the full spark fx rather than having it sandwiched inside the wall the bullet collided with.
			GetTree().CurrentScene.AddChild(newSparks);
		}
		QueueFree();
	}
}
