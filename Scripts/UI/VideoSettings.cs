using Godot;
using System;

public partial class VideoSettings : Control
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

        //if (GetWindow().Mode == Window.ModeEnum.Fullscreen) fullScreenCheck.ButtonPressed = true;
        //They also changed vsync settings to an enum, with some possibly useful new settings. How
        //vsyncCheck.ButtonPressed = OS.VsyncEnabled;
        borderlessCheck.ButtonPressed = GetWindow().Borderless;
        //hdrCheck.ButtonPressed = GetViewport().Hdr;
        framerateCap.Value = Engine.MaxFps;
        physicsRate.Value = Engine.PhysicsTicksPerSecond;
        resolutionScale.Value = GetViewport().Scaling3DScale;
        //FXAASwitch(GetViewport().Fxaa);
        //antiAliasing.Select((int)GetViewport().Msaa3D);
        TargetFPSChange((float)Engine.MaxFps);
        PhysicsRateChange((float)Engine.PhysicsTicksPerSecond);
        //TODO: Toggle switch for AMD FSR //GetViewport().Scaling3DMode = 
    }
    void TargetFPSChange(float value)
    {
        Engine.MaxFps = (int)value;
        fpsCapLabel.Text = value > 0 ? value.ToString() : "Inf";
        bloop.Play();
    }
    void PhysicsRateChange(float value)
    {
        Engine.PhysicsTicksPerSecond = (int)value;
        physicsHzLabel.Text = value.ToString() + "hz";
        bloop.Play();
    }
    void ResolutionScaleChange(float value)
    {
        GetViewport().Scaling3DScale = value;
        resolutionScaleLabel.Text = value.ToString("P0");
        bloop.Play();
    }
    //window options are now an enum, so a toggle switch is a bad idea.
    /*
    private void FullScreenToggle(bool toggled)
    {
        OS.WindowFullscreen = toggled;
    }
    
    private void VsyncToggle(bool toggled)
	{
		OS.VsyncEnabled = toggled;
	}
    */
    private void BorderlessToggle(bool toggled)
    {
        GetWindow().Borderless = toggled;
    }
    /*
    private void HDRToggle(bool toggled)
    {
        GetViewport().Hdr = toggled;
    }
    */
    private void AntiAliasingChange(int id)
    {
        //GetViewport().Msaa3D = (Viewport.Msaa)id;
    }
    private void AnisoChange(int id)
    {
           //Welp turns out I can't easily adjust Anisotropic settings until Godot 4.0 releases. Too bad!
    }
    /*
    private void FXAASwitch(bool toggled)
    {
        GetViewport().Fxaa = toggled;
        fxaaSwitch.Text = toggled ? "FXAA" : "MSAA";
    }
    */
}
