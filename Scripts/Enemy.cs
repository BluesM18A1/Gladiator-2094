using Godot;
using System;

public class Enemy : Combatant
{
    [Export]
    public int bounty = 10;
    public Arena arena;
    public Navigation nav;
    public KinematicBody player;
    public Vector3 playerPos;
    Vector3 pathPos;
    Vector3[] path;
    public float minDistance = 6;
    int pathPoint = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arena = GetTree().GetRoot().GetNode<Arena>("Spatial");
        nav = GetNode<Navigation>("../Navigation");
        head = GetNode<Spatial>("Head");
        player = GetNode<KinematicBody>("../Player");
        playerPos = player.Translation;
        path = nav.GetSimplePath(Translation, playerPos, true);
        pathPos = Translation;
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
        playerPos = nav.GetClosestPoint(player.Translation);
        ProcessInput(delta);
        SetPathPos(playerPos);
    }
    protected void ProcessInput(float delta) //this is where all the AI happens
    {
        Transform camXform = GetGlobalTransform();
        Vector2 inputMovementVector = Vector2.Zero;
        dir = new Vector3();

        if (Input.IsActionJustPressed("ui_accept")) SetPathPos(playerPos);
        
        float distanceToPoint = Translation.DistanceTo(pathPos);
        float distanceToPlayer = Translation.DistanceTo(playerPos);
        if (distanceToPoint > 0.5f && distanceToPlayer > minDistance)
        {
            inputMovementVector = new Vector2 (0,1);
        }
        else inputMovementVector = Vector2.Zero;
        
        LookAt(new Vector3(pathPos.x, Translation.y, pathPos.z), Vector3.Up);
        
        head.LookAt(new Vector3(playerPos.x, playerPos.y + 0.75f, playerPos.z), Vector3.Up);
        inputMovementVector = inputMovementVector.Normalized();
        dir += -camXform.basis.z.Normalized() * inputMovementVector.y;
        dir += camXform.basis.x.Normalized() * inputMovementVector.x;
        
        
    }
    public override void UpdateHealth(int delta)
    {
        //hit sound
        HP += delta;
        if (HP <= 0)
        {
            //die sound
            //spawn a nice explosion maybe
            arena.UpdateScore(bounty);
            QueueFree();
        }
        
    }
    void SetPathPos(Vector3 newPos)
    {
        
        pathPos = path[pathPoint];
        float distanceToPoint = Translation.DistanceTo(pathPos);
        if (distanceToPoint < 0.5f)
        {
            //GD.Print("Target Reached");
            if (pathPoint < path.Length -1)
            {
                //GD.Print("Changing direction");
                pathPoint++;
                pathPos = path[pathPoint];
                //GD.Print(pathPoint.ToString());
            }
            else 
            {
                UpdatePath(newPos);
            }
        }
        
    }
    public void UpdatePath(Vector3 newPos)
    {
        pathPoint = 0;
        path = nav.GetSimplePath(Translation, newPos, true);
    }
    
}
