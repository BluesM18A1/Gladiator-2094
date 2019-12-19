using Godot;
using System;

public class PlayerGun : Gun
{
    [Export]
    public NodePath HUDpath;
    [Export]
    public int ammo = 100;
    private Label ammoNum;
    public bool disabled = false;
    
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
        if (Input.IsActionJustPressed("player_fire1") && !disabled)
        {
            if (ammo > 0)
            {
                //TODO: fire sound
                Fire();
            }
            else
            {
                //TODO: empty clip ticking sound
            }
        }
    }
    protected override void Fire()//this one is gonna need extra parameters when we add multiple weapons
    {
        ammo--;
        ammoNum.Text = ammo.ToString();
        Bullet newBullet = (Bullet)bullet.Instance();
        GetTree().Root.AddChild(newBullet);
        newBullet.GlobalTransform = barrel.GlobalTransform;
        newBullet.friendly = true;
        newBullet.damage = -8;
        newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
    }
    public void AddAmmo(int delta)//this one is gonna need extra parameters when we add multiple weapons
    {
        //TODO: ammo pickup sound
        ammo += delta;
        ammo = Mathf.Clamp(ammo, 0, 999);
        ammoNum.Text = ammo.ToString();
    }
}
