using Godot;
using System;

public class Bullet : RigidBody
{
	float timer = 0;
    [Export]
    public int damage = -1;
    [Export]
    public float speed = 15;
    public bool friendly = false;
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
		if (timer > 10) QueueFree();
    }
	private void _OnCollisionEnter(Node body)
	{
        if (friendly && body.IsInGroup("Players")) //this if statement doesnt really have to exist but if I ever want to make a multiplayer mode and prevent friendly fire, this better exist.
        {
            return;
        }
        if (body.HasMethod("UpdateHealth"))
        {
            Connect(nameof(DealDamage), body, "UpdateHealth");
            EmitSignal(nameof(DealDamage), damage);
        }
        else
        {
            CPUParticles newSparks = (CPUParticles)sparks.Instance();
            GetTree().Root.AddChild(newSparks);
            newSparks.Emitting = true;
            newSparks.Translation = Translation;
            //newSparks.Rotation = Rotation;
        }
    	QueueFree();
	}
    
}

