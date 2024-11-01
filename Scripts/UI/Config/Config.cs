using Godot;
using System;

public partial class Config : Node
{

    [Signal]//for making sure the settings controls update their info when you revert changes
	public delegate void RefreshUIEventHandler(); 
    #region game
    public int difficulty = 0;
    public int score, highScore;
    public byte startWave = 1;
    #endregion
    #region controls
    public float mouseSensitivity = 0.25f;
    public float analogJoypadSensitivity = 1f;
    public Vector2I aimAxisX = new Vector2I(0,2);
	public Vector2I aimAxisY = new Vector2I(0,3);
    public bool invertX = false, invertY = false;
	public float deadzoneX = 0.1f, deadzoneY = 0.1f;
    #endregion
    public override void _Ready()
    {
        LoadConf();
    }
    void RebindInputs(string action, ConfigFile saveConf)
	{
		Godot.Collections.Array binds = (Godot.Collections.Array)saveConf.GetValue("Action Binds", action, InputMap.ActionGetEvents(action));
		//GD.Print(binds);
		InputMap.ActionEraseEvents(action);
		foreach (InputEvent bind in binds)
		{
			InputMap.ActionAddEvent(action, bind);
		}
	}
    public void SaveConf()
    {
        GD.Print("Saving settings....");
        ConfigFile saveConf = new ConfigFile();
        //INPUTS
        foreach (StringName action in InputMap.GetActions())
        {
            if (action.ToString().StartsWith("ui_")) continue;
            saveConf.SetValue("Action Binds", action, InputMap.ActionGetEvents(action));
        }
        saveConf.SetValue("Controls", "InvertX", invertX);
        saveConf.SetValue("Controls", "InvertY", invertY);
        saveConf.SetValue("Controls", "AimAxisX", aimAxisX);
        saveConf.SetValue("Controls", "AimAxisY", aimAxisY);
        saveConf.SetValue("Controls", "DeadzoneX", deadzoneX);
        saveConf.SetValue("Controls", "DeadzoneY", deadzoneY);
        saveConf.SetValue("Controls", "LookSensitivity", mouseSensitivity);
        //
        saveConf.SetValue("Game", "Difficulty", difficulty);
        saveConf.SetValue("Audio", "MasterVolume", AudioServer.GetBusVolumeDb(0));
        saveConf.SetValue("Audio", "BgmVolume", AudioServer.GetBusVolumeDb(1));
        saveConf.SetValue("Audio", "SfxVolume", AudioServer.GetBusVolumeDb(2));
        saveConf.SetValue("Video", "WindowMode", (int)GetWindow().Mode);
        saveConf.SetValue("Video", "VSyncMode", (int)DisplayServer.WindowGetVsyncMode());
        saveConf.SetValue("Video", "Borderless", GetWindow().Borderless);
        saveConf.SetValue("Video", "AntiAliasing", (int)GetViewport().Msaa3D);
        //saveConf.SetValue("Video", "TargetFPS", Engine.MaxFps);
        //saveConf.SetValue("Video", "PhysicsRate", Engine.PhysicsTicksPerSecond);
        saveConf.SetValue("Video", "ResolutionScale", GetViewport().Scaling3DScale);
        saveConf.Save("user://userConfig.ini");
        GD.Print("Saved");
    }
    public void LoadConf()
    {
        GD.Print("Loading settings....");
        ConfigFile saveConf = new ConfigFile();
        saveConf.Load("user://userConfig.ini");
        //INPUTS
        foreach (StringName action in InputMap.GetActions())
        {
            if (action.ToString().StartsWith("ui_")) continue;
            RebindInputs(action, saveConf);
        }
        invertX = (bool)saveConf.GetValue("Controls","InvertX", false);
        invertY = (bool)saveConf.GetValue("Controls","InvertY", false);
        aimAxisX = (Vector2I)saveConf.GetValue("Controls","AimAxisX", new Vector2I(0,2));
        aimAxisY = (Vector2I)saveConf.GetValue("Controls","AimAxisY", new Vector2I(0,3));
        deadzoneX = (float)saveConf.GetValue("Controls","DeadzoneX", 0.1f);
        deadzoneY = (float)saveConf.GetValue("Controls","DeadzoneY", 0.1f);
        //
        mouseSensitivity =  (float)saveConf.GetValue("Controls", "LookSensitivity", mouseSensitivity);
        difficulty = (int)saveConf.GetValue("Game", "Difficulty", difficulty);
        AudioServer.SetBusVolumeDb(0, (float)saveConf.GetValue("Audio", "MasterVolume", AudioServer.GetBusVolumeDb(0))) ;
        AudioServer.SetBusVolumeDb(1, (float)saveConf.GetValue("Audio", "BgmVolume", AudioServer.GetBusVolumeDb(1)));
        AudioServer.SetBusVolumeDb(2, (float)saveConf.GetValue("Audio", "SfxVolume", AudioServer.GetBusVolumeDb(2)));
        GetWindow().Mode = (Window.ModeEnum)(int)saveConf.GetValue("Video", "WindowMode", (int)Window.ModeEnum
        .Windowed);
        DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode)(int)saveConf.GetValue("Video", "VSyncMode", 0));
        GetWindow().Borderless = (bool)saveConf.GetValue("Video", "Borderless", false);
        GetViewport().Msaa3D = (Viewport.Msaa)(int)saveConf.GetValue("Video", "AntiAliasing", 1);
        Engine.PhysicsTicksPerSecond = (int)saveConf.GetValue("Video", "PhysicsRate", 240);
        GetViewport().Scaling3DScale = (float)saveConf.GetValue("Video", "ResolutionScale", 1f);
        GD.Print("Loaded");
        EmitSignal(SignalName.RefreshUI);
    }
    public void LoadDefaults()
    {
        InputMap.LoadFromProjectSettings();
        invertX = false;
        invertY = false;
        aimAxisX = new Vector2I(0,2);
        aimAxisY = new Vector2I(0,3);
        deadzoneX = 0.1f;
        deadzoneY = 0.5f;
        mouseSensitivity = 0.25f;
        difficulty = 0;
        AudioServer.SetBusVolumeDb(0,0);
        AudioServer.SetBusVolumeDb(1,0);
        AudioServer.SetBusVolumeDb(2,0);
        GetWindow().Mode = Window.ModeEnum.Windowed;
        DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
        GetWindow().Borderless = false;
        GetViewport().Msaa3D = Viewport.Msaa.Msaa2X;
        GetViewport().Scaling3DScale = 1f;
        EmitSignal(SignalName.RefreshUI);
    }
}
