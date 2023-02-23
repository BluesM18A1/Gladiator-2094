using Godot;
using System;

public partial class VolumeMeter : ProgressBar
{
    public enum AudioBuses {MASTER = 0, BGM = 1, SFX = 2};
    [Export]
    public AudioBuses bus;
    AudioEffectSpectrumAnalyzerInstance spectrum;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance((int)bus, 0);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        QueueRedraw();
        float magnitude = spectrum.GetMagnitudeForFrequencyRange(0,44100).Length();
        //there is a bug where the visualization will repeat itself if the audio bus is inactive. The workaround has been cited here:
        //https://github.com/godotengine/godot/issues/49250#issuecomment-928078488
        //to prevent this, either increase the channel disable time or decrease the buffer length of the effect. (I am doing the latter)
        
        Value = Mathf.LinearToDb(magnitude) + AudioServer.GetBusVolumeDb((int)bus);
        
    }
}
