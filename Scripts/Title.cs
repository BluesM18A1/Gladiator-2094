using Godot;
using System;

public class Title : Control
{
	Config config;
	AudioStreamPlayer bloop;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			/*if (OS.WindowSize > OS.GetScreenSize())
			{
				OS.WindowSize = OS.GetScreenSize();
				//GD.Print(OS.GetScreenSize());
				//OS.WindowMaximized = true;
			}*/
		config = GetNode<Config>("/root/Config");
		bloop = GetNode<AudioStreamPlayer>("bloop");
		Input.SetMouseMode(Input.MouseMode.Visible);
		//sometimes the window shoots up really high depending on OS and monitor setup. This makes sure you can easily grab and move the window.
		OS.WindowPosition = new Vector2(OS.WindowPosition.x, 0);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void PlayButtonDown()
	{
		GetTree().ChangeScene("res://Scenes/arena.tscn");
	}
	private void OptionsButtonDown()
	{
		GetTree().ChangeScene("res://Scenes/Options.tscn");
	}
	private void CredsButtonDown()
	{
		GetTree().ChangeScene("res://Scenes/Creds.tscn");
	}
	private void ExitButtonDown()
	{
		config.SaveConf();
		GetTree().Quit();
	}
	private void onButtonHover()
	{
		bloop.Play();
	}
	private void BackButtonDown()
	{
		GetTree().ChangeScene("res://Title.tscn");
	}
	public override void _Input(InputEvent @event){
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Key0)
			{
				GetTree().ChangeScene("res://Scenes/debug.tscn");
			}
		}      
	}
}

