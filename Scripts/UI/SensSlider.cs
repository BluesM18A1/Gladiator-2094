using Godot;
using System;

public class SensSlider : HSlider
{
    public AudioStreamPlayer bloop;
    Config config;
    Label setting;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bloop = GetNode<AudioStreamPlayer>("../bloop");
        config = GetNode<Config>("/root/Config");
        setting = GetNode<Label>("Difficulty/Setting");
        Value = config.mouseSensitivity;
        setting.Text = (Value / 0.5f).ToString("P0");
    }
    private void OnSensitivityChange(int delta)
    {
        bloop.Play();
        setting.Text = (Value / 0.5f).ToString("P0");
        config.mouseSensitivity = (float)Value;
    }
}
