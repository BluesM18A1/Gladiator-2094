using Godot;
using System;

public partial class SliderValueLabel : Label
{
    [Export]
    string prefix = "", suffix = "", placevalues = "0.00";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public void ValueUpdate(float value)
    {
        Text = prefix + value.ToString(placevalues) + suffix;
    }
}
