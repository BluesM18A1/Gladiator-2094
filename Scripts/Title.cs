using Godot;
using System;

public class Title : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Visible);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void PlayButtonDown()
	{
    	GetTree().ChangeScene("res://arena.tscn");
	}


	private void ExitButtonDown()
	{
    	GetTree().Quit();
	}
}



