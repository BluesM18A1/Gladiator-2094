using Godot;
using System;

public partial class ButtonRebinder : Button
{
    [Export]
    NodePath menuPath, focusEaterPath;
    //title menu;
    Control focusEater;
    Config config;
    [Export]
    public string inputAction;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
        focusEater = GetNode<Control>(focusEaterPath);
        DisplayKeybinds();
        //SetProcessUnhandledInput(false);
        SetProcessInput(false);
    }
    public override void _Toggled(bool buttonPressed)
    {
        //SetProcessUnhandledInput(buttonPressed);
        SetProcessInput(buttonPressed);
        if (buttonPressed)
        {
            Text = "Recording input...";
            focusEater.Visible = true;
            focusEater.GrabFocus();
            //the following line was me trying to figure out how to make sure having the button hovered over by mouse doesn't confuse rebinder if press m1. No dice. Oh well.
            //ActionMode = ActionModeEnum.Release;
        }
        else
        {
            focusEater.Visible = false;
            GrabFocus();
            DisplayKeybinds();
            //ActionMode = ActionModeEnum.Press;
        }
    }
    public override void _Input(InputEvent @event)
    {
        
        if (@event is InputEventJoypadMotion motion ) 
        {
            if (Mathf.Abs(motion.AxisValue) < 0.5f) return;
            InputMap.ActionAddEvent(inputAction, @event);
            ButtonPressed = false;
        }
        else if (@event is InputEventJoypadButton joyButton) 
        {
            InputMap.ActionAddEvent(inputAction, @event);
            ButtonPressed = false;
        }
        else if (@event is InputEventKey eventKey )
        {
            if (eventKey.Keycode == Key.Escape)
            {
                InputMap.ActionEraseEvents(inputAction);
                ButtonPressed = false;
                return;
            }
            InputMap.ActionAddEvent(inputAction, @event);
            ButtonPressed = false;
        }
        else if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed) 
        {
            InputMap.ActionAddEvent(inputAction, @event);
            ButtonPressed = false;
        }
        
        //if (@event is InputEventMIDI) return; //lmao what if we allowed MIDI input? Would be pretty funny and epic but idk if it's worth the effort to implement. Maybe after the game is finished.
    }
    public void DisplayKeybinds()
    {
        string keymap = "";
        //GD.Print(InputMap.ActionGetEvents(inputAction).Count);
        for (int i = 0; i < InputMap.ActionGetEvents(inputAction).Count; i++)
        {
            
            if (InputMap.ActionGetEvents(inputAction)[i] is InputEventKey keyboardKey)
            {
                keymap += keyboardKey.AsText() + " Key";
                
            }
            else if (InputMap.ActionGetEvents(inputAction)[i] is InputEventMouseButton mouseButton)
            {
                keymap += "Mouse" + mouseButton.ButtonIndex.ToString();
            }
            else if (InputMap.ActionGetEvents(inputAction)[i] is InputEventJoypadButton joyButton)
            {
                keymap += "Joy" +joyButton.Device.ToString() + " Button" + joyButton.ButtonIndex.ToString();
            }
            else if (InputMap.ActionGetEvents(inputAction)[i] is InputEventJoypadMotion joyMotion)
            {
                keymap += "Joy" +joyMotion.Device.ToString() + " Axis" + joyMotion.Axis.ToString() + (joyMotion.AxisValue > 0 ? "+": "-");
            }
            if (i < InputMap.ActionGetEvents(inputAction).Count -1)
            {
                keymap += " / ";
            }
        }
        if (keymap == "") keymap = "None";
        Text = keymap;
        //GD.Print(keymap);
    }

}
