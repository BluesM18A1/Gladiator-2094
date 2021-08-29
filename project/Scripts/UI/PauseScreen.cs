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
                resume_button_pressed();
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
    void button_hovered()
    {
        bloop.Play();
    }
    void exit_button_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Title.tscn");
    }
    void resume_button_pressed()
    {
        Visible = false;
        Input.SetMouseMode(Input.MouseMode.Captured);
        GetTree().Paused = false;
    }
}
