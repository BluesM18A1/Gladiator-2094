using Godot;
using System;

public class Title : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    AudioStreamPlayer bloop;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bloop = GetNode<AudioStreamPlayer>("bloop");
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
    private void OptionsButtonDown()
    {
        GD.Print("WIP");
    }
    private void CredsButtonDown()
    {
        GD.Print("WIP");
    }
	private void ExitButtonDown()
	{
    	GetTree().Quit();
	}
    private void onButtonHover()
    {
        bloop.Play();
    }
}



