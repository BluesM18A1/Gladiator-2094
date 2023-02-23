using Godot;
using System;

public partial class DeadzoneSlider : VSlider
{
    [Export]
    bool xAxis = false;
    Config config;
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
        Refresh();
        Connect("value_changed",new Callable(this,"updateGM"));
    }

    void Refresh()
    {
        Value = xAxis ? config.deadzoneX : config.deadzoneY;
        EmitSignal("value_changed", Value);
    }
    public void updateGM(float value)
    {
        if (xAxis)
        {
            config.deadzoneX = value;
        }
        else config.deadzoneY = value;
    }
}
