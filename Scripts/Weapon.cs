using Godot;
using System;
[Tool]
    public class Weapon : Resource
    {
        [Export]
        public float Timer;
        [Export]
        public PackedScene weapon;
        [Export]
        public int ammo;

        public Weapon(float newTimer, PackedScene newWeapon, sbyte newAmmo){
            Timer = newTimer;
            weapon = newWeapon;
            ammo = newAmmo;
        }
    }


