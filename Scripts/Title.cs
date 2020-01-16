using Godot;
using System;

public class Title : Control
{
    Config config;
    Label diffSetting;
    AudioStreamPlayer bloop;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        diffSetting = GetNode<Label>("DSlider/Difficulty/Setting");
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
        GetTree().ChangeScene("res://Options.tscn");
    }
    private void CredsButtonDown()
    {
        GetTree().ChangeScene("res://Creds.tscn");
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
        GetTree().ChangeScene("res://Title.tscn");
    }
    private void OnDifficultyChange(int delta)
    {
        switch (delta)
        {
            case (-2):
            diffSetting.Text = "Game Journalist";
            break;
            case (-1):
            diffSetting.Text = "Easy";
            break;
            case (-0):
            diffSetting.Text = "Normal";
            break;
            case (1):
            diffSetting.Text = "Hard";
            break;
            case (2):
            diffSetting.Text = "Quake Pro";
            break;
        }
        config.difficulty = delta;
    }
}