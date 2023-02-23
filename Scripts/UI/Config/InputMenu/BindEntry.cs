using Godot;
using System;

public partial class BindEntry : Button
{
    public BindList list;
    public string inputAction;
    [Export] //only exporting for debugging purposes
    public int index;
    public Control focusEater;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DisplayKeybinds();
    }
    public void DisplayKeybinds()
    {
        string keymap;
        switch (InputMap.ActionGetEvents(inputAction)[index])
        {
            case InputEvent n when n is InputEventJoypadMotion joyMotion:
                keymap = "Joy" +joyMotion.Device.ToString() + " Axis" + joyMotion.Axis.ToString() + (joyMotion.AxisValue > 0 ? "+": "-");
            break;
            case InputEvent n when n is InputEventJoypadButton joyButton:
                keymap = "Joy" +joyButton.Device.ToString() + " Button" + joyButton.ButtonIndex.ToString();
            break;
            case InputEvent n when n is InputEventKey eventKey:
                keymap = eventKey.AsText() + " Key";
            break;
            case InputEvent n when n is InputEventMouseButton mouseButton:
                keymap = "Mouse" + mouseButton.ButtonIndex.ToString();
            break;
            default:
                keymap = "None";
            break;
        }
        Text = keymap;
    }
    public override void _Pressed()
    {
        InputMap.ActionEraseEvent(inputAction, (InputEvent)InputMap.ActionGetEvents(inputAction)[index]);

        list.GenerateBindList();
       // QueueFree();
    }
    
}
