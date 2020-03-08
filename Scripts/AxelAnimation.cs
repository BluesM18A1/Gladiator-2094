using Godot;
using System;

public class AxelAnimation : MeshInstance
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

    public override void _Process(float delta)
	{
        if (parent.dir != Vector3.Zero)
        {
            dirRot = (Mathf.Atan2(parent.vel.x, parent.vel.z)) - (parent.Rotation.y); 
        }
        Vector3 hvel = new Vector3 (parent.vel.x, 0, parent.vel.z);
		ani.PlaybackSpeed = -hvel.Length() / 8;
		RotateY(dirRot);
	}

}
