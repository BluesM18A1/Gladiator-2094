using Godot;
using System;

public class EnemyGun : Gun
{
    public AudioStreamPlayer3D snd;
    [Export]
    public float fireRate = 0.5f, time = 0;
    [Export]
    public NodePath parentPath;
    public Enemy parent;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        snd = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        parent = GetNode<Enemy>(parentPath);
        barrel = GetNode<Position3D>("Barrel");
        fireRate = (float)GD.RandRange(0.5f, 0.8f);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ProcessInput(delta);
    }
    protected void ProcessInput(float delta)
    {
        if (time >= fireRate)
        {
            Fire();
            time = 0;
        }
        else
        {
            time += delta;
            fireRate = (float)GD.RandRange(0.5f, 0.8f);
        }
    }
    protected override void Fire()
    {
        //TODO: fire sound
        Bullet newBullet = (Bullet)bullet.Instance();
        GetTree().Root.AddChild(newBullet);
        newBullet.GlobalTransform = barrel.GlobalTransform;
        newBullet.damage = -8;
        newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
        parent.UpdatePath(parent.playerPos);
        snd.PitchScale = fireRate;
        snd.Play();
    }
}
