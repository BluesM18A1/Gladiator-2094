using Godot;
using System;

public partial class Bullet : RigidBody3D
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
	public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/sparks.tscn");
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
	private void _OnCollisionEnter(Node body)
	{
		
		if (body.HasMethod("UpdateHealth"))
		{
			Connect(nameof(DealDamageEventHandler),new Callable(body,"UpdateHealth"));
			EmitSignal(nameof(DealDamageEventHandler), damage);
		}

			Node3D newSparks = (Node3D)sparks.Instantiate();
			newSparks.Position = Position;
			GetTree().Root.AddChild(newSparks);
			
			//newSparks.Rotation = Rotation;
		QueueFree();
	}
	
}

