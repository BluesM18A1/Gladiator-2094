using Godot;
using System;

public class EnemyGun : Gun
{
	[Export]
    public PackedScene bullet = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Bullet.tscn");
	public AudioStreamPlayer3D snd;
	[Export]
	public float fireRateMin = 0.5f, fireRateMax = 0.8f, fireRange = 32;
	float time = 0;
	public float fireRate;
	[Export]
	public bool launchSpeedByDistance = false; //this is for adjusting launch speed for Grenades
	[Export]
	public bool startupDelay = false; //if you have a dual-wielding enemy and want the guns to alternate left and right each cycle
	[Export]
	public bool serialFiring = false; //false for parallel firing (all cylinders at once), true for serial firing (fire one at a time in sequence)
	[Export]
	public NodePath parentPath;
	
	public Spatial player;
	byte serialFireCounter = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		snd = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		player = GetNode<Spatial>("/root/Spatial/Player");
		fireRate = (float)GD.RandRange(fireRateMin, fireRateMax);
		if (startupDelay) time -= fireRate;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (ToGlobal(Translation).DistanceTo(player.Translation) < fireRange)
		{
			if (time >= fireRate)
			{
				if (serialFiring)
				{
					if (serialFireCounter == barrels.Count) serialFireCounter = 0;
					Fire(serialFireCounter);
					serialFireCounter++;
				}
				else
				{
					for (int i = 0; i < barrels.Count; i++)
        	    	{
        	        	Fire(i);
        	    	}
				}
				
				time = 0;
			}
			else
			{
				time += delta;
				fireRate = (float)GD.RandRange(fireRateMin, fireRateMax);
			}
		}
	}
	protected void Fire(int i)
	{
		Bullet newBullet = (Bullet)bullet.Instance();
		newBullet.Transform = GlobalTransform * barrels[i];
		GetTree().Root.AddChild(newBullet);
		newBullet.ApplyImpulse(new Vector3(0, 0, 0),
		 -newBullet.GlobalTransform.basis.z * newBullet.speed * (launchSpeedByDistance ? ToGlobal(Translation).DistanceTo(player.Translation) : 1));
		//parent.GetTarget(); //find a new target
		//parent.UpdatePath(parent.targetNavPos); //start over pathfinding using new target
		//snd.PitchScale = fireRate; // should I use different randoms for pitchscale later?
		if (snd != null) snd.Play();
	}
}
