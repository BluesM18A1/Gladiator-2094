using Godot;
using System;

public class PlayerGun : Gun
{
	
    //public Weapon minigun = new Weapon(3, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Minigun.tscn"));
    //public Weapon buckshot = new Weapon(2, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Buckshot.tscn"));
    //public Weapon grenade = new Weapon(1, (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Grenade.tscn"), 100);
    //GAMEPLAY VARIABLES-----------------------------------------------------------
    [Export]
    public int shells, bullets, grenades;
    [Export]
    public float coolSpeed = 3f, buckshotSpread = 0.06f;
    [Export]
    public float coolRepeater, coolBuckshot, coolGrenades, coolFlames;
    //COMPONENT VARIABLES------------------------------------------------------------
    [Export]
    public NodePath HUDPath;
    [Export]
    public NodePath PlayerPath;
    public Player player;
    public TextureProgress coolMeter;
    public TextureRect iconRptr, iconShot, iconGren, iconFlam; 
    private Label ammoNum;
    public AudioStreamPlayer pickupSndRep, pickupSndShot, pickupSndGren, switchSnd;
    public AudioStreamSample fireGren = 
    (AudioStreamSample)ResourceLoader.Load("res://Sounds/guns/grenadeLaunch.wav"),
    fireShot = (AudioStreamSample)ResourceLoader.Load("res://Sounds/guns/shot.wav"), 
    wepSwitch = (AudioStreamSample)ResourceLoader.Load("res://Sounds/guns/switch.wav");
    public bool disabled = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>(PlayerPath);
        iconRptr = GetNode<TextureRect>(HUDPath + "/WeaponBar/IconRptr");
        iconShot = GetNode<TextureRect>(HUDPath + "/WeaponBar/IconShot");
        iconGren = GetNode<TextureRect>(HUDPath + "/WeaponBar/IconGren");
        iconFlam = GetNode<TextureRect>(HUDPath + "/WeaponBar/IconFlam");
        coolMeter = GetNode<TextureProgress>("Meter/Viewport/TextureProgress");
        barrel = GetNode<Position3D>("Barrel");
        ammoNum = GetNode<Label>(HUDPath + "/FuelMeter/AmmoNum");
		pickupSndRep = GetNode<AudioStreamPlayer>("pickupSnd");
        pickupSndShot = GetNode<AudioStreamPlayer>("pickupSnd");
        pickupSndGren = GetNode<AudioStreamPlayer>("pickupSnd");
        switchSnd = GetNode<AudioStreamPlayer>("switchSnd");
        currentWeapon = Weapons.FLAMETHROWER;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ProcessInput(delta);
        switch (currentWeapon)
        {
            case (Weapons.REPEATER):
            iconRptr.Modulate = new Color(1,1,1,1);
            iconGren.Modulate = new Color(1,1,1,0.5f);
            iconShot.Modulate = new Color(1,1,1,0.5f);
            iconFlam.Modulate = new Color(1,1,1,0.5f);
            coolSpeed = coolRepeater;
            ammoNum.Text = bullets.ToString();
            
            break;
            case (Weapons.BUCKSHOT):
            iconRptr.Modulate = new Color(1,1,1,0.5f);
            iconGren.Modulate = new Color(1,1,1,0.5f);
            iconShot.Modulate = new Color(1,1,1,1);
            iconFlam.Modulate = new Color(1,1,1,0.5f);
            coolSpeed = coolBuckshot;
            ammoNum.Text = shells.ToString();
            break;
            case (Weapons.GRENADES):
            iconRptr.Modulate = new Color(1,1,1,0.5f);
            iconGren.Modulate = new Color(1,1,1,1);
            iconShot.Modulate = new Color(1,1,1,0.5f);
            iconFlam.Modulate = new Color(1,1,1,0.5f);
            coolSpeed = coolGrenades;
            ammoNum.Text = grenades.ToString();
            break;
            case (Weapons.FLAMETHROWER):
            iconRptr.Modulate = new Color(1,1,1,0.5f);
            iconGren.Modulate = new Color(1,1,1,0.5f);
            iconShot.Modulate = new Color(1,1,1,0.5f);
            iconFlam.Modulate = new Color(1,1,1,1);
            coolSpeed = coolFlames;
            int fuelInt = (int)player.fuel;
            ammoNum.Text = fuelInt.ToString();
            break;
                
        }
        coolMeter.Value -= coolSpeed * 100 * delta;
    }
    protected void ProcessInput(float delta)
    {
        if (coolMeter.Value == 0  && !disabled)
        {
            //Which weapon do I fire?
            if (Input.IsActionPressed("player_fire1"))
            {
                switch (currentWeapon)
                {
                    case (Weapons.REPEATER):
                        FireRepeater();
                    break;
                    case (Weapons.BUCKSHOT):
                        FireBuckshot();
                    break;
                    case (Weapons.GRENADES):
                        FireGrenades();
                    break;
                    case (Weapons.FLAMETHROWER):
                        FireFlame();
                    break;
                }
            }
        }
        //FLAMETHROWER RECHARGE MANAGEMENT
        if ((Input.IsActionPressed("player_fire1") && currentWeapon == Weapons.FLAMETHROWER))
        {
            player.flameThrowerOn = true;
        }
        else player.flameThrowerOn = false;
        
    }
    protected void FireRepeater()//this one is gonna need extra parameters when we add multiple weapons
    {
        if (bullets > 0)
        {
            switchSnd.Stream = fireShot;
            switchSnd.Play();
            coolMeter.Value = 100;
            bullets--;
            ammoNum.Text = bullets.ToString();
            PackedScene newWeapon = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Repeater.tscn");
            Bullet newBullet = (Bullet)newWeapon.Instance();
            newBullet.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet);
            newBullet.friendly = true;
            newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
        }
        
    }
    protected void FireBuckshot()//this one is SUPER hacked together for now but I don't see any easier way to implement spread shots at the current moment
    {   //because Godot's C# support isn't complete enough to it the same way I did in the Unity version
        //forgive me master, but this code is going to be super messy.
        if (shells > 0)
        {
            switchSnd.Stream = fireShot;
            switchSnd.Play();
            coolMeter.Value = 100;
            shells--;
            ammoNum.Text = shells.ToString();
            PackedScene newWeapon = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Buckshot.tscn");
            //center
            Bullet newBullet = (Bullet)newWeapon.Instance();
            newBullet.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet);
            newBullet.friendly = true;
            newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
            //left
            Bullet newBullet2 = (Bullet)newWeapon.Instance();
            newBullet2.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet2);
            newBullet2.RotateObjectLocal(new Vector3(0,1,0), buckshotSpread);
            newBullet2.friendly = true;
            newBullet2.ApplyImpulse(new Vector3(0, 0, 0), -newBullet2.GlobalTransform.basis.z * newBullet2.speed);
            //right
            Bullet newBullet3 = (Bullet)newWeapon.Instance();
            newBullet3.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet3);
            newBullet3.RotateObjectLocal(new Vector3(0,1,0), -buckshotSpread);
            newBullet3.friendly = true;
            newBullet3.ApplyImpulse(new Vector3(0, 0, 0), -newBullet3.GlobalTransform.basis.z * newBullet3.speed);
            //up
            Bullet newBullet4 = (Bullet)newWeapon.Instance();
            newBullet4.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet4);
            newBullet4.RotateObjectLocal(new Vector3(1,0,0), buckshotSpread);
            newBullet4.friendly = true;
            newBullet4.ApplyImpulse(new Vector3(0, 0, 0), -newBullet4.GlobalTransform.basis.z * newBullet4.speed);
            //down
            Bullet newBullet5 = (Bullet)newWeapon.Instance();
            newBullet5.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet5);
            newBullet5.RotateObjectLocal(new Vector3(1,0,0), -buckshotSpread);
            newBullet5.friendly = true;
            newBullet5.ApplyImpulse(new Vector3(0, 0, 0), -newBullet5.GlobalTransform.basis.z * newBullet5.speed);
            //down-left
            Bullet newBullet6 = (Bullet)newWeapon.Instance();
            newBullet6.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet6);
            newBullet6.RotateObjectLocal(new Vector3(1,0,0), -buckshotSpread / 1.3f);
            newBullet6.RotateObjectLocal(new Vector3(0,1,0), buckshotSpread / 1.3f);
            newBullet6.friendly = true;
            newBullet6.ApplyImpulse(new Vector3(0, 0, 0), -newBullet6.GlobalTransform.basis.z * newBullet6.speed);
            //up-left
            Bullet newBullet7 = (Bullet)newWeapon.Instance();
            newBullet7.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet7);
            newBullet7.RotateObjectLocal(new Vector3(1,0,0), buckshotSpread / 1.3f);
            newBullet7.RotateObjectLocal(new Vector3(0,1,0), buckshotSpread / 1.3f);
            newBullet7.friendly = true;
            newBullet7.ApplyImpulse(new Vector3(0, 0, 0), -newBullet7.GlobalTransform.basis.z * newBullet7.speed);
            //up-right
            Bullet newBullet8 = (Bullet)newWeapon.Instance();
            newBullet8.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet8);
            newBullet8.RotateObjectLocal(new Vector3(1,0,0), buckshotSpread / 1.3f);
            newBullet8.RotateObjectLocal(new Vector3(0,1,0), -buckshotSpread / 1.3f);
            newBullet8.friendly = true;
            newBullet8.ApplyImpulse(new Vector3(0, 0, 0), -newBullet8.GlobalTransform.basis.z * newBullet8.speed);
            //down-right
            Bullet newBullet9 = (Bullet)newWeapon.Instance();
            newBullet9.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet9);
            newBullet9.RotateObjectLocal(new Vector3(1,0,0), -buckshotSpread / 1.3f);
            newBullet9.RotateObjectLocal(new Vector3(0,1,0), -buckshotSpread / 1.3f);
            newBullet9.friendly = true;
            newBullet9.ApplyImpulse(new Vector3(0, 0, 0), -newBullet9.GlobalTransform.basis.z * newBullet9.speed);
        }
    }
    protected void FireGrenades()
    {
        if (grenades > 0)
        {
            switchSnd.Stream = fireGren;
            switchSnd.Play();
            coolMeter.Value = 100;
            grenades--;
            ammoNum.Text = grenades.ToString();
            PackedScene newWeapon = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Grenades.tscn");
            Bullet newBullet = (Bullet)newWeapon.Instance();
            newBullet.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet);
            newBullet.friendly = true;
            newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
        }
    }
    protected void FireFlame()
    { /*Flames aren't done using the particle system because Godot does not support per-particle collission
        so what we have instead is an extremely rapid fire bullet with short lifespan, effectively limiting its range */
        if (player.fuel > 0)
        {
            coolMeter.Value = 100;
            player.fuel -= 2;
            int fuelInt = (int)player.fuel;
            ammoNum.Text = fuelInt.ToString();
            PackedScene newWeapon = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Flame.tscn");
            Bullet newBullet = (Bullet)newWeapon.Instance();
            newBullet.GlobalTransform = barrel.GlobalTransform;
            GetTree().Root.AddChild(newBullet);
            newBullet.friendly = true;
            newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
        }
    }
    public void AddBullets(int delta)
    {
        pickupSndRep.Play();
        bullets += delta;
        bullets = Mathf.Clamp(bullets, 0, 999);
    }
    public void AddShells(int delta)
    {
        pickupSndShot.Play();
        shells += delta;
        shells = Mathf.Clamp(shells, 0, 999);
    }
    public void AddGrenades(int delta)
    {
        pickupSndGren.Play();
        grenades += delta;
        grenades = Mathf.Clamp(grenades, 0, 999);
    }
    public void NextWeapon()
    {
        if ((byte)currentWeapon >= 4)
        {
            currentWeapon = Weapons.FLAMETHROWER;
        }
        else currentWeapon++;
        switchSnd.Stream = wepSwitch;
        switchSnd.Play();
    }
    public void PrevWeapon()
    {
        if ((byte)currentWeapon <= 1)
        {
            currentWeapon = Weapons.GRENADES;
        }
        else currentWeapon--;
        switchSnd.Stream = wepSwitch;
        switchSnd.Play();
    }
    public override void _Input(InputEvent @event){
        if (coolMeter.Value == 0  && !disabled)
        {
            if  (@event.IsActionReleased("player_nextWep"))
            {
                NextWeapon();
            }
            else if  (@event.IsActionReleased("player_prevWep"))
            {
                PrevWeapon();
            }
        }
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Key1)
            {
                currentWeapon = Weapons.FLAMETHROWER;
                switchSnd.Stream = wepSwitch;
                switchSnd.Play();
            }
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Key2)
            {
                currentWeapon = Weapons.REPEATER;
                switchSnd.Stream = wepSwitch;
                switchSnd.Play();
            }
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Key3)
            {
                currentWeapon = Weapons.BUCKSHOT;
                switchSnd.Stream = wepSwitch;
                switchSnd.Play();
            }
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Key4)
            {
                currentWeapon = Weapons.GRENADES;
                switchSnd.Stream = wepSwitch;
                switchSnd.Play();
            }
        }      
    }
}
