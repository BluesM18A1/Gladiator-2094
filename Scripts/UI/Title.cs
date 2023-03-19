using Godot;
using System;

public partial class Title : Control
{
	[Export]
	Control firstFocus; //so a button doesn't have to be clicked for keyboard/gamepad navigability to kick in
	[Export]
	AudioStreamPlayer hoverSnd, clickSnd;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		firstFocus.GrabFocus();
		//we load the stream in script so you don't hear the sound when grabbing a button's focus on bootup
		hoverSnd.Stream = (AudioStreamWav)ResourceLoader.Load("res://Sounds/UI/button.hover.wav");
	}
	private void PlayButtonDown()
	{
		GetTree().ChangeSceneToFile("res://Arena.tscn");
	}
	private void ExitButtonDown()
	{
		GetTree().Quit();
	}
	private void HoverSnd()
	{
		hoverSnd.Play();
	}
	private void ClickSnd()
	{
		clickSnd.Play();
	}
	public override void _Input(InputEvent @event){
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Key0)
			{
				GetTree().ChangeSceneToFile("res://Debug.tscn");
			}
		}      
	}
}

