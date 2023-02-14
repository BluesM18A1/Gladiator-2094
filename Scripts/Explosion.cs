using Godot;
using System;

public partial class Explosion : Area3D
{
	double timer = 0;
	[Export]
	public int damage = -1;
	[Export]
	public float speed = 15;
	[Export]
	public float lifetime = 15;
	[Export]
	public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/Explosion.tscn");

	[Signal]
	public delegate void DealExplosiveDamageEventHandler(byte damagePoints);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node3D newSparks = (Node3D)sparks.Instantiate();
		GetTree().Root.AddChild(newSparks);
		newSparks.Position = Position;
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
			Connect(nameof(DealExplosiveDamageEventHandler),new Callable(body,"UpdateHealth"));
			EmitSignal(nameof(DealExplosiveDamageEventHandler), damage / f);
			GD.Print(damage / f);
		}
		
	}
}
