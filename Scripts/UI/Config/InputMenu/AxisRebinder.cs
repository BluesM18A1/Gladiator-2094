using Godot;
using System;

public partial class AxisRebinder : Button
{
    [Export]
    Control focusEater;
    Config config;
    [Export]
    public bool yAxis;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"DisplayKeybinds"), (uint)ConnectFlags.ReferenceCounted);
        SetProcessUnhandledInput(false);
        DisplayKeybinds();
    }
    public override void _Toggled(bool buttonPressed)
    {
        SetProcessUnhandledInput(buttonPressed);
        if (buttonPressed)
        {
            Text = "Press ESC to cancel";
            focusEater.Visible = true;
            focusEater.GrabFocus();
        }
        else
        {
            focusEater.Visible = false;
            GrabFocus();
            DisplayKeybinds();
        }
    }
    public void DisplayKeybinds()
    {
        if (yAxis)
        {
            Text = "Joy" + config.aimAxisY.X + " Axis" + config.aimAxisY.Y;
        }
        else
        {   
            Text = "Joy" + config.aimAxisX.X + " Axis" + config.aimAxisX.Y;
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventJoypadMotion motion && Mathf.Abs(motion.AxisValue) > 0.5f)
        {
            if (yAxis)
            {
                config.aimAxisY = new Vector2I(motion.Device, (int)motion.Axis);
            }
            else
            {
                config.aimAxisX = new Vector2I(motion.Device, (int)motion.Axis);
            }
            ButtonPressed = false;
        }
        else if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Escape)
        {
            ButtonPressed = false;
        }

    }
}
