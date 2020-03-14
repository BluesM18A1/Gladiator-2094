using Godot;
using System;

public class fullscreenCheck : CheckBox
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (OS.WindowFullscreen) Pressed = true;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    private void FullScreenToggle(bool toggled)
	{
		OS.WindowFullscreen = toggled;
	}
}
