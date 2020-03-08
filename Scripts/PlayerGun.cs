using Godot;
using System;

public class PlayerGun : Gun
{
	//GAMEPLAY VARIABLES-----------------------------------------------------------
	[Export]
	public int shells, bullets, grenades;
	[Export]
	public float coolSpeed = 3f, buckshotSpread = 0.06f, recoilAnim = 0.002f, recoilOffset = -0.722f;
	[Export]
	public float coolRepeater, coolBuckshot, coolGrenades, coolFlames;
	//COMPONENT VARIABLES------------------------------------------------------------
	PackedScene instanceRepeater = 
	(PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Repeater.tscn");
	PackedScene instanceBuckshot = 
	(PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Buckshot.tscn");
	PackedScene instanceGrenade = 
	(PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Grenades.tscn");
	PackedScene instanceFlame = 
	(PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Player_Flame.tscn");
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
		Translation = new Vector3 (Translation.x, Translation.y, (recoilAnim * (float)coolMeter.Value) + recoilOffset);
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
			
			Bullet newBullet = (Bullet)instanceRepeater.Instance();
			newBullet.Transform = GlobalTransform * barrels[0];
			GetTree().Root.AddChild(newBullet);
			newBullet.friendly = true;
			newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
		}
		
	}
	protected void FireBuckshot()//this one is why I gave the player 9 barrel transforms
	{   
		if (shells > 0)
		{
			switchSnd.Stream = fireShot;
			switchSnd.Play();
			coolMeter.Value = 100;
			shells--;
			ammoNum.Text = shells.ToString();
			for (int i = 0; i < barrels.Count; i++)
            {
				Bullet newBullet = (Bullet)instanceBuckshot.Instance();
				newBullet.Transform = GlobalTransform * barrels[i];
				GetTree().Root.AddChild(newBullet);
				newBullet.friendly = true;
				newBullet.ApplyImpulse(new Vector3(0, 0, 0), -newBullet.GlobalTransform.basis.z * newBullet.speed);
            }
			
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
			
			Bullet newBullet = (Bullet)instanceGrenade.Instance();
			newBullet.Transform = GlobalTransform * barrels[0];
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
			Bullet newBullet = (Bullet)instanceFlame.Instance();
			newBullet.Transform = GlobalTransform * barrels[0];
			//newBullet.Translate(barrels[0].origin);
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
