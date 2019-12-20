using Godot;
using System;

public class Player : Combatant
{
    //PHYSICS VARIABLES--------------------------------
    [Export]
    public float RechargeRate = 60, FuelDrainRate = 60;
    [Export]
    public float JetForce = 4.0f;
    [Export]
    public float MouseSensitivity = 0.3f;
    
    //GAMEPLAY VARIABLES-------------------------------------
    public bool alive = true;
    //COMPONENT VARIABLES------------------------------------
    private Camera camera;
    public PlayerGun gun;
    private TextureProgress healthMeter, fuelMeter;
    private Label healthNum;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");
        camera = GetNode<Camera>("Head/Camera");
        gun = GetNode<PlayerGun>("Head/Gun");
        healthMeter = GetNode<TextureProgress>("HUD/HealthMeter");
        fuelMeter = GetNode<TextureProgress>("HUD/FuelMeter");
        healthNum = GetNode<Label>("HUD/HealthMeter/HealthNum");
        healthNum.Text = healthMeter.Value.ToString();
        Input.SetMouseMode(Input.MouseMode.Captured);
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
        dir = new Vector3();
        Transform camXform = camera.GetGlobalTransform();

        Vector2 inputMovementVector = new Vector2();

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

            //  Jumping
            if (Input.IsActionPressed("player_jump") && fuelMeter.Value > 0)
            {
                vel.y = JetForce;
                fuelMeter.Value -= FuelDrainRate * delta;
            }         
            if (IsOnFloor())
            {   
                if (fuelMeter.Value < 100)
                {
                    fuelMeter.Value += RechargeRate * delta;
                }
            }
        }
        else if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ReloadCurrentScene();
        }
            
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            head.RotateX(Mathf.Deg2Rad(mouseEvent.Relative.y * -MouseSensitivity));
            RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * MouseSensitivity));

            Vector3 cameraRot = head.RotationDegrees;
            cameraRot.x = Mathf.Clamp(cameraRot.x, -85, 85);
            head.RotationDegrees = cameraRot;
        }
    }
    
    public override void UpdateHealth(int delta)
    {
        if (delta < 0)
        {
            //TODO: hurtsound
        }
        else
        {
            //TODO: healsound
        }
        HP += delta;
        HP = Mathf.Clamp(HP, 0, (int)healthMeter.MaxValue);
        healthMeter.Value = HP;
        healthNum.Text = HP.ToString();
        if (HP <= 0)
        {
            //send signal to arena.cs for saying u dead
            alive = false;
            gun.disabled = true;
        }
    }
}
