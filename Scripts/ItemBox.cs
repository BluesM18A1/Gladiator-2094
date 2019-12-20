using Godot;
using System;

public class ItemBox : Area
{
    //GAMEPLAY VARIABLES-------------------------------------
    [Export]
    public byte itemType = 0; //this was going to be an ENUM but exporting enums in C# is currently broken.
    [Export]
    public byte itemCount;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    private void _OnCollisionEnter(Player body)
	{
        if (body.IsInGroup("Players"))
        {
            switch (itemType)
            {
                case (0): //IF HEALTH
                body.UpdateHealth(itemCount);
                break;
                case (1):
                body.gun.AddAmmo(itemCount);
                break;
            }
            QueueFree();
        }
        
	}
}