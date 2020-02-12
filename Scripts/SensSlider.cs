using Godot;
using System;

public class SensSlider : HSlider
{
    Config config;
    Label setting;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        setting = GetNode<Label>("Difficulty/Setting");
        Value = config.mouseSensitivity;
        setting.Text = (Value / 0.5f).ToString("P0");
    }
    private void OnSensitivityChange(int delta)
    {
        setting.Text = (Value / 0.5f).ToString("P0");
        config.mouseSensitivity = (float)Value;
    }
}
