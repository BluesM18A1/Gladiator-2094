using Godot;
using System;

public class Bullet : RigidBody
{
	float timer = 0;
    [Export]
    public int damage = -1;
    [Export]
    public float speed = 15;
    [Export]
    public float lifetime = 15;
    [Signal]
    public delegate void DealDamage(byte damagePoints);
    [Export]
    public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/sparks.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timer+= delta;
		if (timer > lifetime) QueueFree();
    }
	private void _OnCollisionEnter(Node body)
	{
        
        if (body.HasMethod("UpdateHealth"))
        {
            Connect(nameof(DealDamage), body, "UpdateHealth");
            EmitSignal(nameof(DealDamage), damage);
        }

            Spatial newSparks = (Spatial)sparks.Instance();
            newSparks.Translation = Translation;
            GetTree().Root.AddChild(newSparks);
            
            //newSparks.Rotation = Rotation;
    	QueueFree();
	}
    
}

