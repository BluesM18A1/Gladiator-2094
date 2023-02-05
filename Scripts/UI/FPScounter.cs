using Godot;
using System;

public class FPScounter : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    float FPS;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        FPS = Engine.GetFramesPerSecond();
        Text = "FPS:" + FPS.ToString();
        if (FPS > 119)
        {
            SelfModulate = new Color(0,0.5f,1,1);
        }
        else if (FPS > 59)
        {
            SelfModulate = new Color(0, 1, 0, 1);
        }
        else if (FPS > 29)
        {
            SelfModulate = new Color(1, 1, 0, 1);
        }
        else SelfModulate = new Color(1, 0, 0, 1);
    }
}
