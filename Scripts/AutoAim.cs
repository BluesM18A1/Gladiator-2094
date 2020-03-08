using Godot;
using System;

public class AutoAim : Spatial
{
    Transform lookat;
    public Spatial target;
    [Export]//in case we need to offset aim (for grenades or headshots, for example) this might also be useful for interpolation / imperfect tracking
    public float hOffset, vOffset, trackingLatency;
    
    [Export] //clamp values for horizontal and vertical rotation (IN DEGREES)
    public float hMin = -Mathf.Inf, hMax = Mathf.Inf, vMin = -Mathf.Inf, vMax = Mathf.Inf;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        target = GetNode<Spatial>(GetTree().Root.GetNode("Spatial/Player").GetPath()); //I should definitely think of a way to make this modular without sacrificing the ability to easily target the player.
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

        //this is a somewhat rarely used feature in C# thats super useful to remember. Basically goes "bool ? true condition : false condition";
        //float xVal = hAim ? target.Translation.x + hOffset : Translation.x; 
        //float yVal = vAim ? target.Translation.y + vOffset : Translation.y;
        //wait a sec if we're using clamps then we don't even need to have enable flags! ahwell. I'm keeping these commented because they are a good piece of knowledge to reference.

        Vector3 targetPos = new Vector3(target.Translation.x + hOffset, target.Translation.y + vOffset, target.Translation.z); //setting the position for the targetAim to look at
        /* lookat.origin = Transform.origin;
        lookat = lookat.LookingAt(targetPos, Vector3.Up);
        Transform.InterpolateWith(lookat, 1);*/

        //welp, I can't figure out aim interpolation for the life of me. I suppose having all the bullets as projectiles instead of hitscan makes up for the perfect aim.
        LookAt(targetPos, Vector3.Up);
        Vector3 finalRot = Rotation;   
        finalRot.x = Mathf.Clamp(Rotation.x, Mathf.Deg2Rad(vMin), Mathf.Deg2Rad(vMax)); //we want to clamp this first
        finalRot.y = Mathf.Clamp(Rotation.y, Mathf.Deg2Rad(hMin), Mathf.Deg2Rad(hMax));
        
        Rotation = finalRot;
    }
}
