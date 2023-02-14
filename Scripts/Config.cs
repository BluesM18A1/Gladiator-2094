using Godot;
using System;

public partial class Config : Node
{
    #region game
    public int difficulty = 0;
    public int score, highScore;
    public byte startWave = 1;
    #endregion
    #region controls
    public float mouseSensitivity = 0.10f;
    #endregion
    public override void _Ready()
    {
        LoadConf();
    }
    public void SaveConf()
    {
        GD.Print("Saving settings....");
        ConfigFile saveConf = new ConfigFile();
        
        //write these lines
        saveConf.SetValue("Game", "Difficulty", difficulty);
        saveConf.SetValue("Controls", "LookSensitivity", mouseSensitivity);
        saveConf.SetValue("Audio", "MasterVolume", AudioServer.GetBusVolumeDb(0));
        saveConf.SetValue("Audio", "BgmVolume", AudioServer.GetBusVolumeDb(1));
        saveConf.SetValue("Audio", "SfxVolume", AudioServer.GetBusVolumeDb(2));
        //saveConf.SetValue("Video", "Fullscreen", OS.WindowFullscreen);
        //saveConf.SetValue("Video", "VSync", OS.VsyncEnabled);
        saveConf.SetValue("Video", "Borderless", GetWindow().Borderless);
        //saveConf.SetValue("Video", "HDR", GetViewport().Hdr);
        saveConf.SetValue("Video", "AntiAliasing", (int)GetViewport().Msaa3D);
        //saveConf.SetValue("Video", "FXAA", GetViewport().Fxaa);
        saveConf.SetValue("Video", "TargetFPS", Engine.MaxFps);
        saveConf.SetValue("Video", "PhysicsRate", Engine.PhysicsTicksPerSecond);
        saveConf.SetValue("Video", "ResolutionScale", GetViewport().Scaling3DScale);
        saveConf.Save("user://userConfig.ini");
        GD.Print("Saved");
    }
    public void LoadConf()
    {
        GD.Print("Loading settings....");
        ConfigFile saveConf = new ConfigFile();
        saveConf.Load("user://userConfig.ini");
        difficulty = (int)saveConf.GetValue("Game", "Difficulty", difficulty);
        mouseSensitivity =  (float)saveConf.GetValue("Controls", "LookSensitivity", mouseSensitivity);
        AudioServer.SetBusVolumeDb(0, (float)saveConf.GetValue("Audio", "MasterVolume", AudioServer.GetBusVolumeDb(0))) ;
        AudioServer.SetBusVolumeDb(1, (float)saveConf.GetValue("Audio", "BgmVolume", AudioServer.GetBusVolumeDb(1)));
        AudioServer.SetBusVolumeDb(2, (float)saveConf.GetValue("Audio", "SfxVolume", AudioServer.GetBusVolumeDb(2)));
        //OS.WindowFullscreen = (bool)saveConf.GetValue("Video", "Fullscreen", OS.WindowFullscreen);
        //OS.VsyncEnabled = (bool)saveConf.GetValue("Video", "VSync", false);
        GetWindow().Borderless = (bool)saveConf.GetValue("Video", "Borderless", false);
        //GetViewport().Hdr = (bool)saveConf.GetValue("Video", "HDR", false);
        GetViewport().Msaa3D = (Viewport.Msaa)(int)saveConf.GetValue("Video", "AntiAliasing", 1);
        //GetViewport().Fxaa = (bool)saveConf.GetValue("Video", "FXAA", true);
        Engine.MaxFps = (int)saveConf.GetValue("Video", "TargetFPS", 0);
        Engine.PhysicsTicksPerSecond = (int)saveConf.GetValue("Video", "PhysicsRate", 120);
        GetViewport().Scaling3DScale = (float)saveConf.GetValue("Video", "ResolutionScale", 1f);
        //GetViewport().Size = new Vector2(OS.WindowSize.x * resolutionScale, OS.WindowSize.y * resolutionScale);
        GD.Print("Loaded");
        
    }
}
