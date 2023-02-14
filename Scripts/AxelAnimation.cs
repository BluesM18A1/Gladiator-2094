using Godot;
using System;

public partial class AxelAnimation : MeshInstance3D
{
    private Combatant parent;
    private AnimationPlayer ani;
    float dirRot;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetNode<Combatant>(GetParent().GetPath());
        ani = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta)
	{
        if (parent.dir != Vector3.Zero)
        {
            dirRot = (Mathf.Atan2(parent.vel.X, parent.vel.Z)) - (parent.Rotation.Y); 
        }
        Vector3 hvel = new Vector3 (parent.vel.X, 0, parent.vel.Z);
		ani.SpeedScale = -hvel.Length() / 8;
		RotateY(dirRot);
	}

}
