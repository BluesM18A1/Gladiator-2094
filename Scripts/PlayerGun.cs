using Godot;
using System;

public class PlayerGun : Gun
{
    [Export]
    public NodePath HUDpath;
    [Export]
    public byte ammo = 100;
    private Label ammoNum;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        barrel = GetNode<Position3D>("Barrel");
        ammoNum = GetNode<Label>(HUDpath + "/FuelMeter/AmmoNum");
        ammoNum.Text = ammo.ToString();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ProcessInput(delta);
    }
    protected void ProcessInput(float delta)
    {
        if (Input.IsActionJustPressed("player_fire1"))
        {
            if (ammo > 0)
            {
                Fire();
            }
            else
            {
                //empty clip ticking sound
            }
        }
    }
    protected override void Fire()
    {
        ammo--;
        ammoNum.Text = ammo.ToString();
        Bullet newBullet = (Bullet)bullet.Instance();
        GetTree().Root.AddChild(newBullet);
        newBullet.GlobalTransform = barrel.GlobalTransform;
        newBullet.friendly = true;
        newBullet.damage = 8;
        newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
    }
}
