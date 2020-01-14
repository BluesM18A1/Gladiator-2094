using Godot;
using System;

public class ParticleDespawn : CPUParticles
{
    //this is for particle systems that can't despawn automatically
	
	[Export]
  public float lifeSpan = 2;
  float timer = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Emitting = true;
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      timer+= delta;
	    if (timer > lifeSpan) QueueFree();
  }
}
