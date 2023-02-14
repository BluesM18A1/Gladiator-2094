using Godot;
using System;

public partial class Player : Combatant
{
	Config config;
	//PHYSICS VARIABLES--------------------------------
	[Export]
	public double RechargeRate = 100, FuelDrainRate = 60;
	[Export]
	public double JetForce = 3f;
	[Export]
	public double normalSpeed = 7f;
	[Export]
	public double sprintSpeed = 14f;
	[Export]
	public double mouseSensitivity = 0.3f;
	
	//GAMEPLAY VARIABLES-------------------------------------
	public bool alive = true;
	[Export]
	public int maxHP = 100; //in case I want to make max HP/max fuel upgrades a thing later.
	[Export]
	public int maxFuel = 100;
	int HPcounter = 0;
	public double fuel = 0;
	public bool flameThrowerOn = false;
	public double overhealDecrementRate = .25f;
	double overhealTimer = 0;
	bool needToPlayLandSound = false;

	//COMPONENT VARIABLES------------------------------------
	private Camera3D camera;
	public PlayerGun gun;
	private TextureProgressBar healthMeter, fuelMeter;
	private Label healthNum;
	private AnimationPlayer screenAni;
	protected AudioStreamPlayer boostSnd, medSnd, hurtSnd, landSnd;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		config = GetNode<Config>("/root/Config");
		boostSnd = GetNode<AudioStreamPlayer>("boostSnd");
		medSnd = GetNode<AudioStreamPlayer>("medSnd");
		hurtSnd = GetNode<AudioStreamPlayer>("hurtSnd");
		landSnd = GetNode<AudioStreamPlayer>("landSnd");
		head = GetNode<Node3D>("Neck");
		camera = GetNode<Camera3D>("Neck/Camera3D");
		gun = GetNode<PlayerGun>("Neck/Gun");
		healthMeter = GetNode<TextureProgressBar>("HUD/HealthMeter");
		fuelMeter = GetNode<TextureProgressBar>("HUD/FuelMeter");
		healthNum = GetNode<Label>("HUD/HealthMeter/HealthNum");
		screenAni = GetNode<AnimationPlayer>("HUD/ScreenFlash/ScreenTransitions");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		mouseSensitivity = config.mouseSensitivity;
		HP = maxHP;
		healthMeter.MaxValue = maxHP;
		fuelMeter.MaxValue = maxFuel;
		healthNum.Text = HPcounter.ToString();
		healthMeter.Value = HPcounter;
	}
	public override void _Process(double delta)
	{
		// Analog stick aiming
		head.RotateX(Mathf.DegToRad(Input.GetJoyAxis(0, JoyAxis.RightY) * (float)-mouseSensitivity));
		RotateY(Mathf.DegToRad(-Input.GetJoyAxis(0, JoyAxis.RightX)* (float)mouseSensitivity));
		//overheal mechanics
		if (HP > healthMeter.MaxValue)
		{
			//overheal = true;
			overhealTimer += delta;
			if (overhealTimer >= overhealDecrementRate)
			{
				overhealTimer = 0;
				HP--;
				healthNum.Text = HP.ToString();
			}
		}
		//else overheal = false;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		ProcessHealthMeter(delta);
		ProcessMovement(delta); //it is very important that you do MoveAndSlide() before you do
		ProcessInput(delta); //the IsOnFloor() calls that the input processing will do.
	}

	private void ProcessInput(double delta)
	{
		//  Walking
		Transform3D camXform = camera.GlobalTransform;

		Vector2 inputMovementVector = new Vector2();
		
		
			dir = new Vector3();
			if (Input.IsActionPressed("player_forward"))
			inputMovementVector.Y += 1;
			if (Input.IsActionPressed("player_backward"))
				inputMovementVector.Y -= 1;
			if (Input.IsActionPressed("player_left"))
				inputMovementVector.X -= 1;
			if (Input.IsActionPressed("player_right"))
				inputMovementVector.X += 1;
		
		

		inputMovementVector = inputMovementVector.Normalized();

		if (alive)
		{
			//apply movement vector
			dir += -camXform.Basis.Z.Normalized() * inputMovementVector.Y;
			dir += camXform.Basis.X.Normalized() * inputMovementVector.X;
			
			
			//  Jumping / Jetpack thrust
			if (Input.IsActionPressed("player_jump") && fuelMeter.Value > 0)
			{
				if (Input.IsActionJustPressed("player_jump")) boostSnd.Play();
				
				vel.Y = ((float)fuel / 10);
				fuel -= FuelDrainRate * delta;
			}

			//sprinting
			if (Input.IsActionPressed("player_sprint") && fuelMeter.Value > 0)
			{
				if (Input.IsActionJustPressed("player_sprint")) boostSnd.Play();
				
				MaxSpeed = sprintSpeed;
				fuel -= (FuelDrainRate / 3) * delta;
			}
			else MaxSpeed = normalSpeed;

			//stop jetpack sounds
			if ((!Input.IsActionPressed("player_sprint") && !Input.IsActionPressed("player_jump")) || fuelMeter.Value == 0) boostSnd.Stop();
			
			//recharging
			if (IsOnFloor())
			{   
				if (needToPlayLandSound)
				{
					landSnd.Play(); //this doesn't quite work as predictably as I'd like. Oh well.
					needToPlayLandSound = false;
				}
				if (!Input.IsActionPressed("player_sprint") && !flameThrowerOn)
				{
					if (fuel < 100)
					{
						fuel += RechargeRate * delta;
					}
				}
			}
			else needToPlayLandSound = true;
		}
		else
		{
			GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
		}
		fuel = Mathf.Clamp(fuel, 0, 100);
		fuelMeter.Value = fuel;
	}
	public override void _Input(InputEvent @event)
	{
		mouseSensitivity = config.mouseSensitivity;
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
			head.RotateX(Mathf.DegToRad(mouseEvent.Relative.Y * (float)-mouseSensitivity));
			RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * (float)mouseSensitivity));
			//apply vertical clamping to rotation
			Vector3 cameraRot = head.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -85, 85);
			head.RotationDegrees = cameraRot;
		}
	}
	private void ProcessHealthMeter(double delta) //this is how I do health counter rolling
	{
		
		if (HP != HPcounter)
		{
			//flash the color between blue and white every frame
			if (healthMeter.TintProgress == Colors.White)
			{
				healthMeter.TintProgress = new Color(0,0,1,1);
			}
			else healthMeter.TintProgress = new Color(Colors.White);
			//increment/decrement the meter towards the HP value every frame
			if (HP > HPcounter)
			{
				HPcounter++;
				
			}
			else if (HP < HPcounter)
			{
				HPcounter--;
			}
		}
		else
		{
			healthMeter.TintProgress = new Color(0,0,1,1);
		}
		healthNum.Text = HPcounter.ToString();
		healthMeter.Value = HPcounter;
	}
	public override void UpdateHealth(int delta)
	{
		if (delta < 0)
		{
			if (alive)
			{
				hurtSnd.Play();
				screenAni.Play("Hurt");
			}
		}
		else
		{
			fuel = 100;
			medSnd.Play();
		}
		HP += delta;
		//healthMeter.Value = HP;
		//healthNum.Text = HP.ToString();
		if (HP <= 0)
		{
			alive = false;
			gun.disabled = true;
		}
	}
	
}
