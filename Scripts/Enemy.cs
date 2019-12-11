using Godot;
using System;

public class Enemy : Combatant
{
    public KinematicBody player;
    public Vector3 playerPos;
    public float minDistance = 8;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");
        player = GetNode<KinematicBody>("../Player");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
        playerPos = GetNode<KinematicBody>("../Player").Translation;
        ProcessInput(delta);
    }
    protected void ProcessInput(float delta) //this is where all the AI happens
    {
        float distance = Translation.DistanceTo(playerPos);
        dir = new Vector3();
        Transform camXform = GetGlobalTransform();
        Vector2 inputMovementVector = Vector2.Zero;
        if (distance > minDistance)
        {
            inputMovementVector = new Vector2(0,1);
        }
        else inputMovementVector = Vector2.Zero;
        LookAt(new Vector3(playerPos.x, Translation.y, playerPos.z), Vector3.Up);
        inputMovementVector = inputMovementVector.Normalized();
        head.LookAt(new Vector3(playerPos.x, playerPos.y + 0.75f, playerPos.z), Vector3.Up);

        dir += -camXform.basis.z.Normalized() * inputMovementVector.y;
        dir += camXform.basis.x.Normalized() * inputMovementVector.x;
    }
    public override void TakeDamage(byte damage)
    {
        //hit sound
        HP -= damage;
        if (HP <= 0)
        {
            //die sound
            //spawn a nice explosion maybe
            QueueFree();
        }
        
    }
    
}
