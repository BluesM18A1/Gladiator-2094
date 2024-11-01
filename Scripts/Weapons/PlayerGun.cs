using Godot;
using System;

public partial class PlayerGun : Node3D
{
	/*Godot 4.0 at long last allows for custom types to be [Export]ed
	so this script is no longer a fustercluck!
	EDIT: Not possible in C# just yet! https://github.com/godotengine/godot/pull/72619
	Which means I have to construct this array right in the CS file
	I probably should have done something like this from the start... better late than never!
	*/
	//GAMEPLAY VARIABLES-----------------------------------------------------------
	public class Weapon{
		public string name {get;}
		public int ammo {get; set;}
		public float coolSpeed  {get;}
		public Color color {get;}
		public PackedScene instance {get;}
		public AudioStreamWav pickupSnd, fireSnd;
		public TextureRect uiIcon {get; set;}
		public Weapon(string newName, int newAmmo, float newSpeed, Color newColor, PackedScene newInstance, AudioStreamWav newPickupSnd, AudioStreamWav newFireSnd, TextureRect newIcon)
		{
			name = newName;
			ammo = newAmmo;
			coolSpeed = newSpeed;
			color = newColor;
			instance = newInstance;
			pickupSnd = newPickupSnd;
			fireSnd = newFireSnd;
			uiIcon = newIcon;
		}
	}
	Weapon[] weaponList;
	[Export]
	public float recoilAnim = 0.002f, recoilOffset = -0.722f;
	int currentWeapon = 0;
	[Export]
	public NodePath HUDPath;
	[Export]
	Player player {get; set; }
	[Export]
	AnimationPlayer ani{get; set; }
	TextureProgressBar coolMeter;
	Label ammoNum;
	AudioStreamWav emptySnd = (AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/empty.wav");
	AudioStreamPlayer pickupSnd, fireSnd;
	public bool disabled = false;
	sbyte weaponSpinning = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GenerateWeaponList();
		coolMeter = GetNode<TextureProgressBar>("SubViewport/TextureProgressBar");
		ammoNum = GetNode<Label>(HUDPath + "/FuelMeter/AmmoNum");
		pickupSnd = GetNode<AudioStreamPlayer>("pickupSnd");
		fireSnd = GetNode<AudioStreamPlayer>("fireSnd");
		UpdateWeaponData();
		UpdateMeterColor();
	}
	void GenerateWeaponList()
	{
		weaponList = new Weapon[4];
		weaponList[0] = new Weapon(
			"Flamethrower",
			-1, 75, new Color (1,0,0,1), 
			(PackedScene)ResourceLoader.Load("res://Objects/Bullets/Player.Flame.tscn"),
			null,
			null,
			GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconFlam")
			);
		weaponList[1] = new Weapon(
			"Repeater",
			80, 25, new Color (0,0,1,1), 
			(PackedScene)ResourceLoader.Load("res://Objects/Bullets/Player.Repeater.tscn"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/pickup.repeater.wav"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/repeater.wav"),
			GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconRptr")
			);
		weaponList[2] = new Weapon(
			"Buckshot",
			40, 6, new Color (1,1,0,1), 
			(PackedScene)ResourceLoader.Load("res://Objects/Bullets/Player.Buckshot.Package.tscn"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/pickup.buckshot.wav"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/buckshot.wav"),
			GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconShot")
			);
		weaponList[3] = new Weapon(
			"Grenade Launcher",
			20, 3, new Color (0,1,0,1), 
			(PackedScene)ResourceLoader.Load("res://Objects/Bullets/Player.Grenades.tscn"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/pickups/pickup.grenade.wav"),
			(AudioStreamWav)ResourceLoader.Load("res://Sounds/guns/grenade.launch.wav"),
			GetNode<TextureRect>(HUDPath + "/WeaponBar/HBoxContainer/IconGren")
			);
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	double timer = 0;
	public override void _Process(double delta)
	{
		coolMeter.Value -= (50 * weaponList[currentWeapon].coolSpeed) * delta;
		Position = new Vector3 (Position.X, Position.Y, (recoilAnim * (float)coolMeter.Value) + recoilOffset);
		ProcessInput(delta);
		if (currentWeapon == 0)
		{
			int fuelInt = (int)player.fuel;
			ammoNum.Text = fuelInt.ToString();
		}
		//this last block of code is just here to make rapid switching of weapons
		//result in a smooth spinning animation for the gun
		timer += delta;
		if (timer > 0.1 && coolMeter.TintOver != weaponList[currentWeapon].color)
		{
			UpdateMeterColor();
			timer = 0;
		}
	}
	
	protected void ProcessInput(double delta)
	{
		if (coolMeter.Value <= 0  && !disabled)
		{
			//Which weapon do I fire?
			if (Input.IsActionPressed("player_fire"))
			{
				FireWeapon(weaponList[currentWeapon]);
			}
		}
		//FLAMETHROWER RECHARGE MANAGEMENT
		if ((Input.IsActionPressed("player_fire") && currentWeapon == 0))
		{
			player.flameThrowerOn = true;
		}
		else player.flameThrowerOn = false;
		
	}
	void FireWeapon(Weapon weapon)
	{
		if (weapon.ammo == 0)
		{
			if (!fireSnd.Playing)
			{
				fireSnd.Stream = emptySnd;
				fireSnd.Play();
			}
		}
		else 
		{
			if (weapon.fireSnd != null)
			{
				fireSnd.Stream = weapon.fireSnd;
				fireSnd.Play();
			}
			if (weapon.ammo > 0)
			{
				weapon.ammo--;
				ammoNum.Text = weapon.ammo.ToString();
			}
			else if (player.fuel > 0)
			{
				player.fuel += weapon.ammo; //does not simply subtract fuel in case someone wants to come up with a weapon that uses a lot of fuel in 1 go.
			}
			else return;
			coolMeter.Value = 100;
			Node3D newBullet = (Node3D)weapon.instance.Instantiate();
			newBullet.AddToGroup("Player1"); //the string inside will have to be changed when multiplayer comes back. This is for enemies to track who fired the shot that killed them when adding score.
			//this also means that we need to make sure each buckshot pellet in a package also ends up in this group, which is what this foreach loop does in a rather inelegant brute-force manner.
			foreach (Node child in newBullet.GetChildren())
			{
				child.AddToGroup("Player1");
			}
			newBullet.Transform = GlobalTransform;
			newBullet.Position = GlobalPosition - GlobalTransform.Basis.Z * 0.4f;
			GetTree().CurrentScene.AddChild(newBullet);
		}
	}
	public void AddAmmo(int weaponIdx, int delta)
	{
		pickupSnd.Stream = weaponList[weaponIdx].pickupSnd;
		pickupSnd.Play();
		weaponList[weaponIdx].ammo += delta;
		weaponList[weaponIdx].ammo = Mathf.Clamp(weaponList[weaponIdx].ammo, 0, 999);
		UpdateWeaponData();
	}
	public void NextWeapon()
	{
		if ((byte)currentWeapon >= weaponList.Length - 1)
		{
			currentWeapon = 0;
		}
		else currentWeapon++;
		UpdateWeaponData();
		if (ani.IsPlaying() && ani.CurrentAnimation == "rotate_backwards")
		{
			weaponSpinning = -1;
		}
		else ani.Play("rotate_backwards");
	}
	public void PrevWeapon()
	{
		if ((byte)currentWeapon == 0)
		{
			currentWeapon = weaponList.Length - 1;
		}
		else currentWeapon--;
		UpdateWeaponData();
		if (ani.IsPlaying() && ani.CurrentAnimation == "rotate")
		{
			weaponSpinning = 1;
		}
		else ani.Play("rotate");
	}
	public void UpdateWeaponData()
	{
		for (int i = 0; i < weaponList.Length; i++)
		{
			if (i == currentWeapon)
			{
				weaponList[i].uiIcon.Modulate = new Color(1,1,1,1);
			}
			else weaponList[i].uiIcon.Modulate = new Color(1,1,1,0.5f);
		}
		ammoNum.Text = weaponList[currentWeapon].ammo.ToString();
	}
	public void UpdateMeterColor() //called by animation
	{	
		if (weaponSpinning > 0)
		{
			ani.Play("RESET");
			ani.Play("rotate");
			weaponSpinning = 0;
		}
		if (weaponSpinning < 0)
		{
			ani.Play("RESET");
			ani.Play("rotate_backwards");
			weaponSpinning = 0;
		}
		coolMeter.TintOver = weaponList[currentWeapon].color;
	}

	public override void _Input(InputEvent @event){
		if (coolMeter.Value == 0  && !disabled)
		{
			if  (@event.IsActionPressed("player_next_weapon"))
			{
				NextWeapon();
			}
			else if  (@event.IsActionPressed("player_prev_weapon"))
			{
				PrevWeapon();
			}
			if (@event.IsActionPressed("player_flamethrower"))
			{
				currentWeapon = 0;
				UpdateWeaponData();
				ani.Play("RESET");
				ani.Play("rotate");
			}
			if (@event.IsActionPressed("player_repeater"))
			{
				currentWeapon = 1;
				UpdateWeaponData();
				ani.Play("RESET");
				ani.Play("rotate");
			}
			if (@event.IsActionPressed("player_buckshot"))
			{
				currentWeapon = 2;
				UpdateWeaponData();
				ani.Play("RESET");
				ani.Play("rotate");
			}
			if (@event.IsActionPressed("player_grenades"))
			{
				currentWeapon = 3;
				UpdateWeaponData();
				ani.Play("RESET");
				ani.Play("rotate");
			}
		}
	}
}
