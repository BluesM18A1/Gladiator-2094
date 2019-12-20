using Godot;
using System;

public class Combatant : KinematicBody
{

    //PHYSICS VARIABLES--------------------------------
    [Export]
    public float Gravity = -20f;
    [Export]
    public float MaxSpeed = 8.0f;
    [Export]
    public float Accel = 4.5f;
    [Export]
    public float Deaccel = 16.0f;
    [Export]
    public float MaxSlopeAngle = 50.0f;
    //GAMEPLAY VARIABLES-------------------------------------
    [Export]
    public int HP = 100;

    //COMPONENT VARIABLES------------------------------------
    protected Spatial head;
    protected Vector3 vel = new Vector3();
    protected Vector3 dir = new Vector3();
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        ProcessMovement(delta);
    }
    protected virtual void ProcessMovement(float delta)
    {
        dir.y = 0;
        dir = dir.Normalized();

        vel.y += delta * Gravity;
        
        Vector3 hvel = vel;
        hvel.y = 0;

        Vector3 target = dir;

        target *= MaxSpeed;

        float accel;
        if (dir.Dot(hvel) > 0)
            accel = Accel;
        else
            accel = Deaccel;

        hvel = hvel.LinearInterpolate(target, accel * delta);
        vel.x = hvel.x;
        vel.z = hvel.z;
        vel = MoveAndSlide(vel, new Vector3(0, 1, 0), false, 4, Mathf.Deg2Rad(MaxSlopeAngle));
    }
    public virtual void UpdateHealth(int damage)
    {
        GD.Print("owie" + damage.ToString());
        //TODO: hit sound
        HP -= damage;
    }
}
