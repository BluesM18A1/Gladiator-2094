using Godot;
using System;

public class Config : Node
{
    #region game
    public int difficulty = 0;
    public int score, highScore;
    public byte startWave = 1;
    #endregion
    #region controls
    public float mouseSensitivity = 0.10f;
    #endregion
    #region video
    bool vsync = false;
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
        saveConf.SetValue("Video", "Fullscreen", OS.WindowFullscreen);
        saveConf.SetValue("Video", "VSync", vsync);
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
        OS.WindowFullscreen = (bool)saveConf.GetValue("Video", "Fullscreen", OS.WindowFullscreen);
        GD.Print("Loaded");
    }
}
