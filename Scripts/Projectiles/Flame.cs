using Godot;
using System;

public partial class Flame : CharacterBody3D
{
	double timer = 0;
	[Export]
	public int damage = -1;
	[Export]
	public float speed = 15, randomSpread = 15f;
	[Export]
	public double lifetime = 15;
	[Signal]
	public delegate void DealDamageEventHandler(int damagePoints);
	[Export]
	public PackedScene sparks;
	public override void _Ready()
	{
		RotationDegrees += new Vector3(
			(float)GD.RandRange(-randomSpread, randomSpread),
			(float)GD.RandRange(-randomSpread, randomSpread),
			(float)GD.RandRange(-randomSpread, randomSpread));
		
	}
	public override void _Process(double delta)
	{
		
		timer+= delta;
		if (timer > lifetime) QueueFree();
	}
	public override void _PhysicsProcess(double delta)
	{
		Velocity += -GlobalTransform.Basis.Z * (float)(speed * delta);
		MoveAndSlide();
	}
	private void _OnCollisionEnter(Node body)
	{
		
		if (body.HasMethod("UpdateHealth"))
		{
			//body.Call("UpdateHealth", damage);
			Connect(SignalName.DealDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
			EmitSignal(SignalName.DealDamage, damage, GetGroups()[0].ToString());
		}
		if (sparks != null)
		{
			Node3D newSparks = (Node3D)sparks.Instantiate();
			newSparks.Position = Position;
			GetTree().CurrentScene.AddChild(newSparks);
		}
		QueueFree();
	}
}
