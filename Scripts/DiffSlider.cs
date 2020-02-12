using Godot;
using System;

public class DiffSlider : HSlider
{
    Config config;
    Label diffSetting;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        diffSetting = GetNode<Label>("Difficulty/Setting");
        Value = config.difficulty;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
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
