using Godot;
using System;

public partial class ConfigFileControlButton : Button
{
	enum Mode {LOADDEFAULT, LOADCONF, SAVECONF}
	[Export]
	Mode mode;
	public override void _Ready()
	{
		Config config = GetNode<Config>("/root/Config");
		switch (mode)
		{
			case (Mode.LOADDEFAULT):
			Connect(SignalName.Pressed, new Callable(config, "LoadDefaults"));
			break;
			case (Mode.LOADCONF):
			Connect(SignalName.Pressed, new Callable(config, "LoadConf"));
			break;
			case (Mode.SAVECONF):
			Connect(SignalName.Pressed, new Callable(config, "SaveConf"));
			break;

		}
	}

}
