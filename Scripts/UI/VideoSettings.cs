using Godot;
using System;

public class VideoSettings : Control
{
    Config cfg;
    CheckBox fullScreenCheck, vsyncCheck, borderlessCheck, hdrCheck;
    Button fxaaSwitch;
    OptionButton antiAliasing, anisotropic;
    Slider framerateCap, physicsRate, resolutionScale;
    Label fpsCapLabel, physicsHzLabel, resolutionScaleLabel;
    AudioStreamPlayer bloop;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cfg = GetNode<Config>("/root/Config");
        framerateCap = GetNode<Slider>("TargetFPS");
        fpsCapLabel = GetNode<Label>("TargetFPS/Label/FPS");
        physicsRate = GetNode<Slider>("PhysicsRate");
        physicsHzLabel = GetNode<Label>("PhysicsRate/Label/Hz");
        resolutionScale = GetNode<Slider>("ResolutionScale");
        resolutionScaleLabel = GetNode<Label>("ResolutionScale/Label/Value");
        fullScreenCheck = GetNode<CheckBox>("Fullscreen");
        vsyncCheck = GetNode<CheckBox>("VSync");
        borderlessCheck = GetNode<CheckBox>("Borderless");
        hdrCheck = GetNode<CheckBox>("HDR");
        fxaaSwitch = GetNode<Button>("AntiAliasing");
        antiAliasing = GetNode<OptionButton>("AntiAliasing/Level");
        anisotropic = GetNode<OptionButton>("Aniso/Level");
        bloop = GetNode<AudioStreamPlayer>("bloop");
        UpdateUI();
    }

    void UpdateUI()
    {
        fullScreenCheck.Pressed = OS.WindowFullscreen;
        vsyncCheck.Pressed = OS.VsyncEnabled;
        borderlessCheck.Pressed = OS.WindowBorderless;
        hdrCheck.Pressed = GetViewport().Hdr;
        framerateCap.Value = Engine.TargetFps;
        physicsRate.Value = Engine.IterationsPerSecond;
        resolutionScale.Value = cfg.resolutionScale;
        FXAASwitch(GetViewport().Fxaa);
        antiAliasing.Select((int)GetViewport().Msaa);
        TargetFPSChange((float)Engine.TargetFps);
        PhysicsRateChange((float)Engine.IterationsPerSecond);
        ResolutionScaleChange(cfg.resolutionScale);
        GetViewport().Size = new Vector2(OS.WindowSize.x * cfg.resolutionScale, OS.WindowSize.y * cfg.resolutionScale);
    }
    void TargetFPSChange(float value)
    {
        Engine.TargetFps = (int)value;
        fpsCapLabel.Text = value > 0 ? value.ToString() : "Inf";
        bloop.Play();
    }
    void PhysicsRateChange(float value)
    {
        Engine.IterationsPerSecond = (int)value;
        physicsHzLabel.Text = value.ToString() + "hz";
        bloop.Play();
    }
    void ResolutionScaleChange(float value)
    {
        cfg.resolutionScale = value;
        resolutionScaleLabel.Text = value.ToString("P0");
        GetViewport().Size = new Vector2(OS.WindowSize.x * cfg.resolutionScale, OS.WindowSize.y * cfg.resolutionScale);
        bloop.Play();
    }
    private void FullScreenToggle(bool toggled)
    {
        OS.WindowFullscreen = toggled;
    }
    private void VsyncToggle(bool toggled)
	{
		OS.VsyncEnabled = toggled;
	}
    private void BorderlessToggle(bool toggled)
    {
        OS.WindowBorderless = toggled;
    }
    private void HDRToggle(bool toggled)
    {
        GetViewport().Hdr = toggled;
    }
    private void AntiAliasingChange(int id)
    {
        GetViewport().Msaa = (Viewport.MSAA)id;
    }
    private void AnisoChange(int id)
    {
           //Welp turns out I can't easily adjust Anisotropic settings until Godot 4.0 releases. Too bad!
    }
    private void FXAASwitch(bool toggled)
    {
        GetViewport().Fxaa = toggled;
        fxaaSwitch.Text = toggled ? "FXAA" : "MSAA";
    }
}
