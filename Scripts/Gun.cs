using Godot;
using System;

public class Gun : MeshInstance
{
    //GAMEPLAY VARIABLES-------------------------------------

    //COMPONENT VARIABLES------------------------------------
    [Export]
    public PackedScene bullet = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullet.tscn");
    protected Position3D barrel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        barrel = GetNode<Position3D>("Barrel");
    }
    
    protected virtual void Fire()
    {
        
    }
}