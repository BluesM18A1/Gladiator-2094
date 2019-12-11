using Godot;
using System;

public class Bullet : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public byte damage = 1;
    [Export]
    public float speed = 15;
    public bool friendly = false;
    [Signal]
    public delegate void DealDamage(byte damagePoints);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
            
    }
	private void _OnCollisionEnter(Node body)
	{
        if (friendly && body.GetType().Name == "Player")
        {
            return;
        }
        if (body.HasMethod("TakeDamage"))
        {
            Connect(nameof(DealDamage), body, "TakeDamage");
            EmitSignal(nameof(DealDamage), damage);
        }
    	QueueFree();
	}
    
}

