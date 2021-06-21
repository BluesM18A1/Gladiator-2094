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
            case (-3):
            diffSetting.Text = "Game Journalist";
            break;
            case (-2):
            diffSetting.Text = "Beginner";
            break;
            case (-1):
            diffSetting.Text = "Novice";
            break;
            case (-0):
            diffSetting.Text = "Intermediate";
            break;
            case (1):
            diffSetting.Text = "Adept";
            break;
            case (2):
            diffSetting.Text = "Hardcore";
            break;
            case (3):
            diffSetting.Text = "Masochist";
            break;
        }
        config.difficulty = delta;
    }
}
