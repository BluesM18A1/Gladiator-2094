using Godot;
using System;

public partial class Combatant : CharacterBody3D
{
    //GAMEPLAY VARIABLES-------------------------------------
    [Export]
    public int HP = 100;
    [ExportGroup("Movement Physics")]
    [Export]
    public float Gravity = -20f;
    [Export]
    public double MaxSpeed = 8.0f;
    [Export]
    public float Accel = 4.5f;
    [Export]
    public float Deaccel = 16.0f;
    [Export]
    public double MaxSlopeAngle = 50.0f;
    

    //COMPONENT VARIABLES------------------------------------
    protected Node3D head;
    public Vector3 vel = new Vector3();
    public Vector3 dir = new Vector3();
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        ProcessMovement(delta);
    }
    protected virtual void ProcessMovement(double delta)
    {
        dir.Y = 0;
        dir = dir.Normalized();

        vel.Y += (float)delta * Gravity;
        
        Vector3 hvel = vel;
        hvel.Y = 0;

        Vector3 target = dir;

        target *= (float)MaxSpeed;

        float accel;
        if (dir.Dot(hvel) > 0)
            accel = Accel;
        else
            accel = Deaccel;

        hvel = hvel.Lerp(target, accel * (float)delta);
        vel.X = hvel.X;
        vel.Z = hvel.Z;
        
        //protip for people adapting their code from 3.x,
        //what used to be this:
        //vel = MoveAndSlide(vel, new Vector3(0, 1, 0), false, 4, Mathf.DegToRad(MaxSlopeAngle));
        //is now the following 3 lines, and the parameters from moveandslide are now editable from the insepctor.
        //if you don't include the third line, 
        //gravity will be constantly applied when you are on the floor 
        //so you drop like a rock falling of ledges and can't climb slopes properly.
        Velocity = vel;
        MoveAndSlide();
        vel = GetRealVelocity();
        //if (IsOnFloor()) vel.Y += (float)delta;
        
        
    }
    public virtual void UpdateHealth(int damage)
    {
        GD.Print("owie" + damage.ToString());
        //TODO: hit sound
        HP -= damage;
    }
}
