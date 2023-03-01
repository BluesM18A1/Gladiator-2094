using Godot;
using System;

public partial class Title : Control
{
	AudioStreamPlayer bloop;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bloop = GetNode<AudioStreamPlayer>("bloop");
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void PlayButtonDown()
	{
		GetTree().ChangeSceneToFile("res://arena.tscn");
	}
	private void ExitButtonDown()
	{
		GetTree().Quit();
	}
	private void onButtonHover()
	{
		bloop.Play();
	}
	private void BackButtonDown()
	{
		GetTree().ChangeSceneToFile("res://Title.tscn");
	}
	public override void _Input(InputEvent @event){
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Key0)
			{
				GetTree().ChangeSceneToFile("res://debug.tscn");
			}
		}      
	}
}

