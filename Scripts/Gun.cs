using Godot;
using System;

public partial class Gun : Node3D
{
    //GAMEPLAY VARIABLES-------------------------------------
    public enum Weapons {REPEATER = 2, BUCKSHOT = 3, GRENADES = 4, FLAMETHROWER = 1};
    [Export]
    public Weapons currentWeapon;
    //COMPONENT VARIABLES------------------------------------
    
    
    [Export((PropertyHint) 13)] //I'm gonna specify array length using the inspector https://github.com/godotengine/godot/issues/15467#issuecomment-487341656
    public Godot.Collections.Array<Transform3D> barrels;
    
}