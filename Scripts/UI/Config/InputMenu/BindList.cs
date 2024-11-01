using Godot;
using System;

public partial class BindList : Control
{
    Config config;
    [Export]
    public string inputAction;
    [Export]
    public Label label;
    [Export]
    public Control focusEater, addButton;
    [Export]
    HBoxContainer list;
    [Export]
    public PackedScene bindEntry, bindPlaceholder;
    Control newBindPlaceholder;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"GenerateBindList"), (uint)ConnectFlags.ReferenceCounted);
        GenerateBindList();
        SetProcessInput(false);
    }
    public void GenerateBindList()
    {
        int index = 0;
        foreach (Node child in list.GetChildren())
        {
            child.QueueFree();
            index++;
        }
        //if (index > 0) GD.Print("Cleared "+ index.ToString() + " binds");
        index = 0;
        foreach (InputEvent actionEvent in InputMap.ActionGetEvents(inputAction))
        {
            AddBind(index);
            index++;
        }
        //if (index > 0) GD.Print("Added "+ index.ToString() + " binds");
        CalibrateFocus();
    }
    public void CalibrateFocus() //automatically generates the focus neighbors for keyboard/gamepad navigation
    {
        for (int i = 0; i < list.GetChildCount(); i++)
        {
            Control newEntry = (Control)list.GetChild(i);
            if (i > 0)
            {
                Control previousEntry = (Control)list.GetChild(i - 1);
                previousEntry.FocusNeighborRight = previousEntry.GetPathTo(newEntry);
                newEntry.FocusNeighborLeft = newEntry.GetPathTo(previousEntry); 
            }
            newEntry.FocusNeighborRight = newEntry.GetPathTo(addButton);
            addButton.FocusNeighborLeft = addButton.GetPathTo(newEntry);
        }
    }
    public async void AddBind(int index)
    {
        BindEntry newEntry = (BindEntry)bindEntry.Instantiate();
        newEntry.inputAction = inputAction;
        newEntry.index = index;
        newEntry.list = this;
        //else GD.Print(index);
        list.AddChild(newEntry, true);
        //list.CallDeferred("AddChild", newEntry, true);
        await ToSignal(GetTree().Root, "ready");
        //newEntry.Connect("tree_exited",new Callable(this,"CalibrateFocus")); //
    }
    public void NewBind()
    {
        newBindPlaceholder = (Control)bindPlaceholder.Instantiate();
        list.AddChild(newBindPlaceholder);
        newBindPlaceholder.GrabFocus();
        SetProcessInput(true);
        focusEater.Visible = true;
        focusEater.GrabFocus();
        //once read, kill the placeholder box and regenerate the bind list. Boom, new entry.
    }
    public override void _Input(InputEvent @event)
    {
        switch (@event){
            case InputEvent n when n is InputEventJoypadMotion motion:
                if (Mathf.Abs(motion.AxisValue) < 0.5f) return;
                InputMap.ActionAddEvent(inputAction, @event);
                CloseNewBind();
            break;
            case InputEvent n when n is InputEventJoypadButton joyButton:
                InputMap.ActionAddEvent(inputAction, @event);
                CloseNewBind();
            break;
            case InputEvent n when n is InputEventKey eventKey:
                InputMap.ActionAddEvent(inputAction, @event);
                CloseNewBind();
            break;
            case InputEvent n when n is InputEventMouseButton mouseButton:
                InputMap.ActionAddEvent(inputAction, @event);
                CloseNewBind();
            break;
            case InputEvent n when n is InputEventMidi:
            //this would be the funniest thing ever to me if I actually implement this, but it's probably not worth the trouble. 
            //"YEAH! SHOOTING DUCKS WITH A PIANO!" -AVGN
            break;
            default:
            break;
        }
        
    }
    void CloseNewBind()
    {
        focusEater.Visible = false;
        newBindPlaceholder.QueueFree();
        SetProcessInput(false);
        GenerateBindList();
        addButton.GrabFocus();
    }
    
}
