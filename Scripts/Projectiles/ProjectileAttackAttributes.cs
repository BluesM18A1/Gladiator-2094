using Godot;
using System;
[GlobalClass]
public partial class ProjectileAttackAttributes : Node
{
    Node3D host;
    [Export]
	public int damage = -1;
	[Export]
	public float speed = 100, randomSpread = 0f;
	double timer = 0;
	[Export]
	public double lifetime = 15;
	[Signal]
	public delegate void DealDamageEventHandler(int damagePoints);
	[Export]
	public PackedScene onCollisionFX;
    public override void _Ready()
	{
        host = GetParent<Node3D>();
		host.RotationDegrees += new Vector3(
			(float)GD.RandRange(-randomSpread, randomSpread),
			(float)GD.RandRange(-randomSpread, randomSpread),
			(float)GD.RandRange(-randomSpread, randomSpread));
        if (host is RigidBody3D rb3d) //rigidbody3d movement behavior is defined here instead of in physicsprocess because... yea
        {
            rb3d.ApplyImpulse(-rb3d.GlobalTransform.Basis.Z * speed);
        }
	}
    public override void _Process(double delta)
	{
		timer+= delta;
		if (timer > lifetime) host.QueueFree();
	}
    public override void _PhysicsProcess(double delta)
	{
        //passive movement handling based on rotation, behavior altered depending on host type
        if (host is CharacterBody3D cb3d)
        {
            cb3d.Velocity += -cb3d.GlobalTransform.Basis.Z * (float)(speed * delta);
		    cb3d.MoveAndSlide();
        }
        else if (host is Area3D a3d)
        {
            a3d.Position -= a3d.Transform.Basis.Z * (float)(speed * delta);
        }
	}
    private void _OnCollisionEnter(Node3D body)
	{
		if (body.HasMethod("UpdateHealth"))
		{
			//body.Call("UpdateHealth", damage);
			Connect(SignalName.DealDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
			EmitSignal(SignalName.DealDamage, damage, host.GetGroups()[0].ToString());
		}
		if (onCollisionFX != null)
		{
			Node3D newSparks = (Node3D)onCollisionFX.Instantiate();
			newSparks.Position = host.GlobalPosition + host.Transform.Basis.Z * 0.4f; //the offset is so we can see the full spark fx rather than having it sandwiched inside the wall the bullet collided with.
			newSparks.AddToGroup(host.GetGroups()[0]); //making sure that children such as explosive damage areas are in the same group for tagging
            GetTree().CurrentScene.AddChild(newSparks);
		}
		host.QueueFree();
	}
}