using Godot;
using System;

public class EnemyGun : Gun
{
    [Export]
    public float fireRate = 0.5f, time = 0;
    [Export]
    public NodePath parentPath;
    public Enemy parent;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetNode<Enemy>(parentPath);
        barrel = GetNode<Position3D>("Barrel");
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
        else time += delta;
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
    }
}
