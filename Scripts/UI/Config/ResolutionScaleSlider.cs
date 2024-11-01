using Godot;
using System;

public partial class ResolutionScaleSlider : HSlider
{
	[Export]
	Label label;
	Config config;
	public override void _Ready()
	{
		config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
		Refresh();
	}
	void ValueChange(float value)
    {
        GetViewport().Scaling3DScale = value;
        label.Text = value.ToString("P0");
    }
	void Refresh()
	{
		Value = GetViewport().Scaling3DScale;
		label.Text = Value.ToString("P0");
	}
}
