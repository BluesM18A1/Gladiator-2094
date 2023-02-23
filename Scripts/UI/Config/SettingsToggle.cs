using Godot;
using System;
public partial class SettingsToggle : Button
{
	//this enum type might have to be migrated to a new script that handles all custom resource types later.
	enum Settings {FULLSCREEN, BORDERLESS, INVERTAIMX, INVERTAIMY}
	[Export]
	Settings setting;
	Config config;
	public override void _Ready()
	{
		config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
		Refresh();
	}
	void Refresh()
	{
		switch (setting)
		{
			case Settings.FULLSCREEN:
				ButtonPressed = GetWindow().Mode == Window.ModeEnum.Fullscreen ? true : false;
			break;
			case Settings.BORDERLESS:
				ButtonPressed = GetWindow().Borderless;
			break;
			case Settings.INVERTAIMX:
				ButtonPressed = config.invertX;
			break;
			case Settings.INVERTAIMY:
				ButtonPressed = config.invertY;
			break;
		}
	}
	public override void _Toggled(bool buttonPressed)
	{
		switch (setting)
		{
			case Settings.FULLSCREEN:
				GetWindow().Mode = ButtonPressed ? Window.ModeEnum.Fullscreen : Window.ModeEnum.Windowed;
			break;
			case Settings.BORDERLESS:
				GetWindow().Borderless = buttonPressed;
			break;
			case Settings.INVERTAIMX:
				config.invertX = buttonPressed;
			break;
			case Settings.INVERTAIMY:
				config.invertY = buttonPressed;
			break;
		}
	}
}
