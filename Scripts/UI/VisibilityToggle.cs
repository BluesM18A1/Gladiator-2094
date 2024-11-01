using Godot;
using System;

public partial class VisibilityToggle : Button
{
	[Export]
	Control control;
	[Export]
	Control focus;
	public override void _Ready()
	{
		Connect(SignalName.Pressed, new Callable(this, "OnPress"));
	}
	void OnPress()
	{
		control.Visible = !control.Visible;
		focus.GrabFocus();
	}	

}
