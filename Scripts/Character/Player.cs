using Godot;
using System;

public partial class Player : Combatant
{
	Config config;
	[ExportGroup("Player Stats")]
	public bool alive = true;
	[Export]
	public int maxHP = 100; //in case I want to make max HP/max fuel upgrades a thing later.
	[Export]
	public int maxFuel = 100;
	[Export]
	public double RechargeRate = 100, FuelDrainRate = 50;
	[ExportGroup("Movement Speeds")]
	float hoverPower = 100;
	[Export]
	public double normalSpeed = 12f;
	[Export]
	public double sprintSpeed = 24f;
	
	
	int HPcounter = 0;
	public double fuel = 0;
	public bool flameThrowerOn = false;
	public double overhealDecrementRate = .25f;
	double overhealTimer = 0;
	bool needToPlayLandSound = false;
	
	[Signal]
	public delegate void PlayerDeathEventHandler();
	//COMPONENT VARIABLES------------------------------------
	Arena arena;
	private Camera3D camera;
	public PlayerGun gun;
	private RayCast3D floorChecker;
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
		floorChecker = GetNode<RayCast3D>("floorchecker");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		HP = maxHP;
		healthMeter.MaxValue = maxHP;
		fuelMeter.MaxValue = maxFuel;
		healthNum.Text = HPcounter.ToString();
		healthMeter.Value = HPcounter;
	}
	public override void _Process(double delta)
	{
		// Analog stick aiming
		float xAxis = config.invertX ? 
			Input.GetJoyAxis(config.aimAxisX.X, (JoyAxis)config.aimAxisX.Y) 
			: -Input.GetJoyAxis(config.aimAxisX.X, (JoyAxis)config.aimAxisX.Y);
        float yAxis = config.invertY ? 
			Input.GetJoyAxis(config.aimAxisY.X, (JoyAxis)config.aimAxisY.Y) 
			: -Input.GetJoyAxis(config.aimAxisY.X, (JoyAxis)config.aimAxisY.Y);
		if (Mathf.Abs(xAxis) > config.deadzoneX)
		{
			RotateY(Mathf.DegToRad(xAxis)* config.mouseSensitivity * 1000 * (float)delta);
		}
		if (Mathf.Abs(yAxis) > config.deadzoneY) 
		{
			head.RotateX(Mathf.DegToRad(yAxis) * config.mouseSensitivity * 1000 *(float)delta);
		}
		Vector3 cameraRot = head.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -85, 85);
			head.RotationDegrees = cameraRot;
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
		if (alive)ProcessMovement(delta); //it is very important that you do MoveAndSlide() before you do
		ProcessInput(delta); //the IsOnFloor() calls that the input processing will do.
	}
	public void SendToSpectatorZone()
	{
		Position = new Vector3(32,16,32);
		Rotation = new Vector3(0,230,0);
		head.Rotation = Vector3.Zero;

	}
	private void ProcessInput(double delta)
	{
		//recharging
			if (IsOnFloor() || floorChecker.IsColliding())
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
			else
			{
				if (!floorChecker.IsColliding()) needToPlayLandSound = true;
			}
		//  Walking
		Transform3D camXform = camera.GlobalTransform;

		Vector2 inputMovementVector = new Vector2();
		
		dir = new Vector3();
		inputMovementVector = new Vector2(
			Input.GetAxis("player_strafe_left", "player_strafe_right"), 
			Input.GetAxis("player_move_backward","player_move_forward"));

		if (inputMovementVector.Length() > 1)
			inputMovementVector = inputMovementVector.Normalized();

		if (alive)
		{
			//apply movement vector
			dir += -camXform.Basis.Z.Normalized() * inputMovementVector.Y;
			dir += camXform.Basis.X.Normalized() * inputMovementVector.X;
			
			
			//  Jumping / Jetpack thrust
			if (Input.IsActionPressed("player_jump") && fuelMeter.Value > 0)
			{
				if (Input.IsActionJustPressed("player_jump"))
				{
					if (IsOnFloor() || floorChecker.IsColliding())
						vel.Y = (10);
					boostSnd.Play();
				}
				else vel.Y += ((float)fuel / 3 * (float)delta);
				//vel.Y = (((float)fuel / 10));
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
		}
		else
		{
			//
		}
		fuel = Mathf.Clamp(fuel, 0, 100);
		fuelMeter.Value = fuel;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
			head.RotateX(Mathf.DegToRad(mouseEvent.Relative.Y * -config.mouseSensitivity / 2));
			RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * config.mouseSensitivity / 2));
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
	public override void UpdateHealth(int delta, string tag)
	{
		if (HP <= 0) return; //so the death check doesn't happen twice
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
			EmitSignal(SignalName.PlayerDeath);
			screenAni.Play("Death");
			Input.MouseMode = Input.MouseModeEnum.Visible;
			gun.disabled = true;
		}
	}
	
}
