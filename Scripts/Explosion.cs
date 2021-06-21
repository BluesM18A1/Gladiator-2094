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
    [Export]
    public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/Explosion.tscn");

    [Signal]
    public delegate void DealExplosiveDamage(byte damagePoints);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Spatial newSparks = (Spatial)sparks.Instance();
        GetTree().Root.AddChild(newSparks);
        newSparks.Translation = Translation;
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
            Spatial target = (Spatial)body;
            Vector3 diff = target.Translation - Translation;
		    float f = Mathf.Sqrt((diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z));
            f = f - 1; //adding 1 unit of tolerance for max damage
            f = Mathf.Clamp(f,1,5); //making sure the damage reduction doesnt go in negatives or too low
            Connect(nameof(DealExplosiveDamage), body, "UpdateHealth");
            EmitSignal(nameof(DealExplosiveDamage), damage / f);
            GD.Print(damage / f);
        }
        
	}
}
