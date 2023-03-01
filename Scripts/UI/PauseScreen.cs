using Godot;
using System;

public partial class PauseScreen : Control
{
	[Export]
	Player player;
	AudioStreamPlayer bloop;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bloop = GetNode<AudioStreamPlayer>("bloop");
	}
	public override void _Process(double delta)
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
				Input.MouseMode = Input.MouseModeEnum.Visible;
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
		GetTree().ChangeSceneToFile("res://Title.tscn");
	}
	void ResumeButtonDown()
	{
		Visible = false;
		if (player.alive) Input.MouseMode = Input.MouseModeEnum.Captured;
		GetTree().Paused = false;
	}
}
