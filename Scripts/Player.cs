using Godot;
using System;

public class Player : Combatant
{
	Config config;
	//PHYSICS VARIABLES--------------------------------
	[Export]
	public float RechargeRate = 100, FuelDrainRate = 60;
	[Export]
	public float JetForce = 3f;
	[Export]
	public float normalSpeed = 7f;
	[Export]
	public float sprintSpeed = 14f;
	[Export]
	public float mouseSensitivity = 0.3f;
	
	//GAMEPLAY VARIABLES-------------------------------------
	public bool alive = true;
	public bool flameThrowerOn = false;
	public float fuel = 100;
	//COMPONENT VARIABLES------------------------------------
	private Camera camera;
	public PlayerGun gun;
	private TextureProgress healthMeter, fuelMeter;
	private Label healthNum;
	private AnimationPlayer screenAni;
	protected AudioStreamPlayer boostSnd, medSnd, hurtSnd;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		config = GetNode<Config>("/root/Config");
		boostSnd = GetNode<AudioStreamPlayer>("boostSnd");
		medSnd = GetNode<AudioStreamPlayer>("medSnd");
		hurtSnd = GetNode<AudioStreamPlayer>("hurtSnd");
		head = GetNode<Spatial>("Neck");
		camera = GetNode<Camera>("Neck/Camera");
		gun = GetNode<PlayerGun>("Neck/Gun");
		healthMeter = GetNode<TextureProgress>("HUD/HealthMeter");
		fuelMeter = GetNode<TextureProgress>("HUD/FuelMeter");
		healthNum = GetNode<Label>("HUD/HealthMeter/HealthNum");
		screenAni = GetNode<AnimationPlayer>("HUD/ScreenFlash/ScreenTransitions");
		healthNum.Text = healthMeter.Value.ToString();
		Input.SetMouseMode(Input.MouseMode.Captured);
		mouseSensitivity = config.mouseSensitivity;
	}

	public override void _PhysicsProcess(float delta)
	{
		
		ProcessInput(delta);
		ProcessMovement(delta);
	}

	private void ProcessInput(float delta)
	{
		//  Capturing/Freeing the cursor
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			if (Input.GetMouseMode() == Input.MouseMode.Visible)
				 Input.SetMouseMode(Input.MouseMode.Captured);
			else
				Input.SetMouseMode(Input.MouseMode.Visible);
		}
		
		
		//  Walking
		Transform camXform = camera.GlobalTransform;

		Vector2 inputMovementVector = new Vector2();
		
		
			dir = new Vector3();
			if (Input.IsActionPressed("player_forward"))
			inputMovementVector.y += 1;
			if (Input.IsActionPressed("player_backward"))
				inputMovementVector.y -= 1;
			if (Input.IsActionPressed("player_left"))
				inputMovementVector.x -= 1;
			if (Input.IsActionPressed("player_right"))
				inputMovementVector.x += 1;
		
		

		inputMovementVector = inputMovementVector.Normalized();

		if (alive)
		{
			//apply movement vector
			dir += -camXform.basis.z.Normalized() * inputMovementVector.y;
			dir += camXform.basis.x.Normalized() * inputMovementVector.x;
			
			
			//  Jumping / Jetpack thrust
			if (Input.IsActionPressed("player_jump") && fuelMeter.Value > 0)
			{
				if (Input.IsActionJustPressed("player_jump")) boostSnd.Play();
				
				vel.y = (fuel / 10);
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
			if (IsOnFloor() && !Input.IsActionPressed("player_sprint") && !flameThrowerOn)
			{   
				if (fuel < 100)
				{
					fuel += RechargeRate * delta;
				}
			}
		}
		else
		{
			GetTree().ChangeScene("res://Scenes/GameOver.tscn");
		}
		fuel = Mathf.Clamp(fuel, 0, 100);
		fuelMeter.Value = fuel;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
		{
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
			head.RotateX(Mathf.Deg2Rad(mouseEvent.Relative.y * -mouseSensitivity));
			RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * mouseSensitivity));

			Vector3 cameraRot = head.RotationDegrees;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -85, 85);
			head.RotationDegrees = cameraRot;
		}
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
			medSnd.Play();
		}
		HP += delta;
		HP = Mathf.Clamp(HP, 0, (int)healthMeter.MaxValue);
		healthMeter.Value = HP;
		healthNum.Text = HP.ToString();
		if (HP <= 0)
		{
			alive = false;
			gun.disabled = true;
		}
	}
	
}
