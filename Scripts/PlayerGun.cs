using Godot;
using System;

public class PlayerGun : Gun
{
    public AudioStreamPlayer fireSnd;
	public AudioStreamPlayer pickupSnd;
    public Weapon minigun = new Weapon(3, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullet.tscn"), 100);
    public Weapon buckshot = new Weapon(2, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullet.tscn"), 100);
    public Weapon grenade = new Weapon(1, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullet.tscn"), 100);
    public byte currentWeapon;
    public TextureProgress coolMeter;
    [Export]
    public NodePath HUDpath;
    [Export]
    public int ammo = 100;
    [Export]
    public float coolSpeed = 3f;
    private Label ammoNum;
    public bool disabled = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        coolMeter = GetNode<TextureProgress>("Meter/Viewport/TextureProgress");
        barrel = GetNode<Position3D>("Barrel");
        ammoNum = GetNode<Label>(HUDpath + "/FuelMeter/AmmoNum");
        ammoNum.Text = ammo.ToString();
        fireSnd = GetNode<AudioStreamPlayer>("fireSnd");
		pickupSnd = GetNode<AudioStreamPlayer>("pickupSnd");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ProcessInput(delta);
        coolMeter.Value -= coolSpeed;
    }
    protected void ProcessInput(float delta)
    {
        if (Input.IsActionPressed("player_fire1") && !disabled && coolMeter.Value == 0)
        {
            if (ammo > 0)
            {
                coolMeter.Value = 100;
                Fire();
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
        newBullet.damage = -5;
        newBullet.speed = 50;
        newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
        fireSnd.Play();
    }
    public void AddAmmo(int delta)//this one is gonna need extra parameters when we add multiple weapons
    {
        pickupSnd.Play();
        ammo += delta;
        ammo = Mathf.Clamp(ammo, 0, 999);
        ammoNum.Text = ammo.ToString();
    }
}
