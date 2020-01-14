using Godot;
using System;

public class Explosion : Area
{
    float timer = 0;
    [Export]
    public int damage = -1;
    [Export]
    public float speed = 15;
    [Export]
    public float lifetime = 15;
    public bool friendly = false;
    [Export]
    public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/Explosion.tscn");

    [Signal]
    public delegate void DealExplosiveDamage(byte damagePoints);
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
        if (friendly && body.IsInGroup("Players")) //this if statement doesnt really have to exist but if I ever want to make a multiplayer mode and prevent friendly fire, this better exist.
        {
            return;
        }
        if (body.HasMethod("UpdateHealth"))
        {
            Connect(nameof(DealExplosiveDamage), body, "UpdateHealth");
            EmitSignal(nameof(DealExplosiveDamage), damage);
        }
        Spatial newSparks = (Spatial)sparks.Instance();
            GetTree().Root.AddChild(newSparks);
            newSparks.Translation = Translation;
	}
}
