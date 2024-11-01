using Godot;
using System;

public partial class SettingsEnumOptions : OptionButton
{
	enum Mode{VSYNC, MSAA}
	[Export]
	Mode mode;
	Config config;
	public override void _Ready()
	{
		config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
		
		Refresh();
	}
	void Refresh()
	{
		switch (mode)
		{
			case Mode.VSYNC:
				Selected = (int)DisplayServer.WindowGetVsyncMode();
			break;
			case Mode.MSAA:
				Selected = (int)GetViewport().Msaa3D;
			break;
		}
	}
	void _ItemSelect(int index)
	{
		switch (mode)
		{
			case Mode.VSYNC:
				DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode)index);
			break;
			case Mode.MSAA:
				GetViewport().Msaa3D = (Viewport.Msaa)index;
			break;
		}
	}
}
