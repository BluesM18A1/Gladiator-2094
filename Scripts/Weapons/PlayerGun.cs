using Godot;
using System;

public partial class PlayerGun : Gun
{
	/*Godot 4.0 at long last allows for custom types to be [Export]ed
	this giant spaghetti code mess is not for long!
	
	EDIT: Not possible in C# just yet! https://github.com/godotengine/godot/pull/72619
	*/
	//GAMEPLAY VARIABLES-----------------------------------------------------------
	/*
	public struct Weapon{
		int ammo;
		float coolSpeed;
		PackedScene instance;
		AudioStreamWav fireSnd;
	}*/
	[Export]
	public int shells, bullets, grenades;
	[Export]
	public float recoilAnim = 0.002f, recoilOffset = -0.722f;
	[Export]
	public float coolRepeater, coolBuckshot, coolGrenades, coolFlames;
	float coolSpeed = 3f;
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
	Player player {get; set; }
	[Export]
	AnimationPlayer ani{get; set; }
	TextureProgressBar coolMeter;
	TextureRect iconRptr, iconShot, iconGren, iconFlam; 
	Label ammoNum;
	AudioStreamPlayer pickupSnd, fireSnd;
	AudioStreamWav fireGren = 
	(AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/grenadeLaunch.wav"),
	fireRep = (AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/repeater.wav"), 
	fireShot = (AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/buckshot.wav");
	AudioStreamWav pickupGren = 
	(AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/grenadePickup.wav"),
	pickupRep = (AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/repeaterPickup.wav"), 
	pickupShot = (AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/buckshotPickup.wav");
	public bool disabled = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		coolMeter = GetNode<TextureProgressBar>("SubViewport/TextureProgressBar");
		iconRptr = GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconRptr");
		iconShot = GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconShot");
		iconGren = GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconGren");
		iconFlam = GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconFlam");
		ammoNum = GetNode<Label>(HUDPath + "/FuelMeter/AmmoNum");
		pickupSnd = GetNode<AudioStreamPlayer>("pickupSnd");
		fireSnd = GetNode<AudioStreamPlayer>("fireSnd");
		currentWeapon = Weapons.FLAMETHROWER;
		UpdateWeaponData();
		UpdateMeterColor();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		coolMeter.Value -= (50 * coolSpeed) * delta;
		Position = new Vector3 (Position.X, Position.Y, (recoilAnim * (float)coolMeter.Value) + recoilOffset);
		ProcessInput(delta);
		if (currentWeapon == Weapons.FLAMETHROWER)
		{
			int fuelInt = (int)player.fuel;
			ammoNum.Text = fuelInt.ToString();
		}
		
	}
	/*public override void _PhysicsProcess(float delta)
	{
		
		//no idea why this needs to happen in _PhysicsProcess, 
		//but the meter jams if I decrement it by coolSpeed * delta.
		//and no other timers in the game malfunction when using delta but this one???
	}*/
	
	protected void ProcessInput(double delta)
	{
		if (coolMeter.Value == 0  && !disabled)
		{
			//Which weapon do I fire?
			if (Input.IsActionPressed("player_fire"))
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
		if ((Input.IsActionPressed("player_fire") && currentWeapon == Weapons.FLAMETHROWER))
		{
			player.flameThrowerOn = true;
		}
		else player.flameThrowerOn = false;
		
	}
	
	protected void FireRepeater()//this one is gonna need extra parameters when we add multiple weapons
	{
		if (bullets > 0)
		{
			fireSnd.Stream = fireRep;
			fireSnd.Play();
			coolMeter.Value = 100;
			bullets--;
			ammoNum.Text = bullets.ToString();
			
			Node3D newBullet = (Node3D)instanceRepeater.Instantiate();
			newBullet.Transform = GlobalTransform * barrels[0];
			GetTree().CurrentScene.AddChild(newBullet);
			//newBullet.ApplyImpulse(-newBullet.GlobalTransform.Basis.Z * newBullet.speed);
		}
		
	}
	protected void FireBuckshot()//this one is why I gave the player 9 barrel transforms
	{   
		if (shells > 0)
		{
			fireSnd.Stream = fireShot;
			fireSnd.Play();
			coolMeter.Value = 100;
			shells--;
			ammoNum.Text = shells.ToString();
			for (int i = 0; i < barrels.Count; i++)
			{
				Node3D newBullet = (Node3D)instanceBuckshot.Instantiate();
				newBullet.Transform = GlobalTransform * barrels[i];
				GetTree().CurrentScene.AddChild(newBullet);
				//newBullet.ApplyImpulse(-newBullet.GlobalTransform.Basis.Z * newBullet.speed);
			}
			
		}
	}
	protected void FireGrenades()
	{
		if (grenades > 0)
		{
			fireSnd.Stream = fireGren;
			fireSnd.Play();
			coolMeter.Value = 100;
			grenades--;
			ammoNum.Text = grenades.ToString();
			
			Node3D newBullet = (Node3D)instanceGrenade.Instantiate();
			newBullet.Transform = GlobalTransform * barrels[0];
			GetTree().CurrentScene.AddChild(newBullet);
			//newBullet.ApplyImpulse(-newBullet.GlobalTransform.Basis.Z * newBullet.speed);
		}
	}
	protected void FireFlame()
	{ /*Flames aren't done using the particle system because Godot does not support per-particle collission
		so what we have instead is an extremely rapid fire bullet with short lifespan, effectively limiting its range */
		if (player.fuel > 0)
		{
			coolMeter.Value = 100;
			player.fuel--;
			int fuelInt = (int)player.fuel;
			ammoNum.Text = fuelInt.ToString();
			Node3D newBullet = (Node3D)instanceFlame.Instantiate();
			newBullet.Transform = GlobalTransform * barrels[0];
			//newBullet.Translate(barrels[0].origin);
			GetTree().CurrentScene.AddChild(newBullet);
			//newBullet.ApplyImpulse(-newBullet.GlobalTransform.Basis.Z * newBullet.speed);
		}
	}
	public void AddBullets(int delta)
	{
		pickupSnd.Stream = pickupRep;
		pickupSnd.Play();
		bullets += delta;
		bullets = Mathf.Clamp(bullets, 0, 999);
		UpdateWeaponData();
	}
	public void AddShells(int delta)
	{
		pickupSnd.Stream = pickupShot;
		pickupSnd.Play();
		shells += delta;
		shells = Mathf.Clamp(shells, 0, 999);
		UpdateWeaponData();
	}
	public void AddGrenades(int delta)
	{
		pickupSnd.Stream = pickupGren;
		pickupSnd.Play();
		grenades += delta;
		grenades = Mathf.Clamp(grenades, 0, 999);
		UpdateWeaponData();
	}
	public void NextWeapon()
	{
		if ((byte)currentWeapon >= 4)
		{
			currentWeapon = Weapons.FLAMETHROWER;
		}
		else currentWeapon++;
		UpdateWeaponData();
		ani.Play("RESET");
		ani.Play("rotate_backwards");
	}
	public void PrevWeapon()
	{
		if ((byte)currentWeapon <= 1)
		{
			currentWeapon = Weapons.GRENADES;
		}
		else currentWeapon--;
		UpdateWeaponData();
		ani.Play("RESET");
		ani.Play("rotate");
		
	}
	public void UpdateWeaponData()
	{
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
	}
	public void UpdateMeterColor() //called by animation
	{
		switch (currentWeapon)
		{
			case (Weapons.REPEATER):
			coolMeter.TintOver = new Color (0,0,1,1);
			break;
			case (Weapons.BUCKSHOT):
			coolMeter.TintOver = new Color (1,1,0,1);
			break;
			case (Weapons.GRENADES):
			coolMeter.TintOver = new Color (0,1,0,1);
			break;
			case (Weapons.FLAMETHROWER):
			coolMeter.TintOver = new Color (1,0,0,1);
			break;
				
		}
	}

	public override void _Input(InputEvent @event){
		if (coolMeter.Value == 0  && !disabled)
		{
			if  (@event.IsActionReleased("player_next_weapon"))
			{
				NextWeapon();
			}
			else if  (@event.IsActionReleased("player_prev_weapon"))
			{
				PrevWeapon();
			}
		}
		
		if (@event.IsActionPressed("player_flamethrower"))
		{
			currentWeapon = Weapons.FLAMETHROWER;
			UpdateWeaponData();
			ani.Play("RESET");
			ani.Play("rotate");
		}
		if (@event.IsActionPressed("player_repeater"))
		{
			currentWeapon = Weapons.REPEATER;
			UpdateWeaponData();
			ani.Play("RESET");
			ani.Play("rotate");
		}
		if (@event.IsActionPressed("player_buckshot"))
		{
			currentWeapon = Weapons.BUCKSHOT;
			UpdateWeaponData();
			ani.Play("RESET");
			ani.Play("rotate");
		}
		if (@event.IsActionPressed("player_grenades"))
		{
			currentWeapon = Weapons.GRENADES;
			UpdateWeaponData();
			ani.Play("RESET");
			ani.Play("rotate");
		}
		 
	}
}
