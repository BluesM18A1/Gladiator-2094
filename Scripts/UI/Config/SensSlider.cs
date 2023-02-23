using Godot;
using System;

public partial class SensSlider : HSlider
{
    //public AudioStreamPlayer bloop;
    Config config;
    [Export]
    Label setting;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
        Refresh();
    }
    void ValueChange(float value)
    {
        //bloop.Play();
        setting.Text = (Value / 0.5f).ToString("P0");
        config.mouseSensitivity = (float)Value;
    }
    void Refresh()
    {
        Value = config.mouseSensitivity;
        setting.Text = (Value / 0.5f).ToString("P0");
    }
}
