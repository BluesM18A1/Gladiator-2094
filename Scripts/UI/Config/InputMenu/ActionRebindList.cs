using Godot;
using System;
using System.Globalization;

public partial class ActionRebindList : VBoxContainer
{
	//This will help generate a list of all* available actions in your project settings
	//For your in-game input rebinding menu.
	[Export]
    Control focusEater;
	[Export]
    public PackedScene rebindRow;

	[Export]
	Godot.Collections.Array<String> blacklist, filterNames; 
	//Filters out actions by name. For example...
	//The blacklist might contain "ui_" to keep out the engine's built-in actions
	//And filterNames may contain "player_", not necessary but highly encouranged since the filterNames string(s) will be removed from the label text for neatness and brevity.
	//You could still add something specific like "player_secretbuttonorsomething" to the hypothetical blacklist after this and hide just that one action from the "player_" set

	public override void _Ready()
	{
		GenerateActionList();
	}
	public void GenerateActionList()
    {
        for (int i = 0; i < InputMap.GetActions().Count; i++)
        {
			bool blockflag = false;
			StringName name = InputMap.GetActions()[i];
			for (int j = 0; j < blacklist.Count; j++)
			{
				if (name.ToString().StartsWith(blacklist[j])) blockflag = true;
			}
			if (!blockflag) AddAction(name);
        }
    }
	async void AddAction(Godot.StringName action)
	{
		BindList newEntry = (BindList)rebindRow.Instantiate();
        newEntry.inputAction = action;
		newEntry.label.Text = PrettyifyName(action);
		
        newEntry.focusEater = focusEater;
		AddChild(newEntry, true);
        await ToSignal(GetTree().Root, "ready");
        //newEntry.Connect("tree_exited",new Callable(this,"CalibrateFocus")); //
        //CalibrateFocus();
	}
	string PrettyifyName(string input)
	{
		string s = input;
		for (int i = 0; i < filterNames.Count; i++)
		{
			if (s.StartsWith(filterNames[i]))
			{
				s = TrimStart(input, filterNames[i]);
			}
		}
		s = s.Replace("_"," ");
		s = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		return s;
	}
	string TrimStart(string source, string toTrim)
	{
		string s = source;
		while (s.StartsWith(toTrim))
		{
			s = s.Substring(toTrim.Length);
		}
		return s;
	}
	

}
