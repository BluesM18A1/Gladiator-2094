using Godot;
using System;

public class Gun : MeshInstance
{
    //GAMEPLAY VARIABLES-------------------------------------
    public enum Weapons {REPEATER = 2, BUCKSHOT = 3, GRENADES = 4, FLAMETHROWER = 1};
    [Export]
    public Weapons currentWeapon;
    //COMPONENT VARIABLES------------------------------------
    [Export]
    public PackedScene bullet = (PackedScene)ResourceLoader.Load("res://Prefabs/Bullets/Bullet.tscn");
    protected Position3D barrel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        barrel = GetNode<Position3D>("Barrel");
    }
    
}