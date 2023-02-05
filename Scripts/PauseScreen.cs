using Godot;
using System;

public class PauseScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    AudioStreamPlayer bloop;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bloop = GetNode<AudioStreamPlayer>("bloop");
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
		{
            if (GetTree().Paused ==  true)
			{
                GD.Print("unpausing....");
                    ResumeButtonDown();
			}
            else 
            {
                GD.Print("pausing....");
				Input.SetMouseMode(Input.MouseMode.Visible);
				Visible = true;
				GetTree().Paused = true;
            }
		}
    }
    void onButtonHover()
    {
        bloop.Play();
    }
    void QuitButtonDown()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Title.tscn");
    }
    void ResumeButtonDown()
    {
        Visible = false;
        Input.SetMouseMode(Input.MouseMode.Captured);
        GetTree().Paused = false;
    }
}
