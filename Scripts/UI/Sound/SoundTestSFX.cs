using Godot;
using System;
public partial class SoundTestSFX : Button
{
    [Export]
    Godot.Collections.Array<AudioStream> fileList;
    Texture2D play = (Texture2D)ResourceLoader.Load("res://UI/icons/play.webp"),
    stop = (Texture2D)ResourceLoader.Load("res://UI/icons/stop.webp");
    [Export]
    AudioStreamPlayer _asp;
    byte selection = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("pressed",new Callable(this,"_onPress"));
        //_asp.Connect("finished",new Callable(this,"_finishPlayback"));
        Icon = play;
    }

    void _onPress()
    {
            _asp.Stop();
            _asp.Stream = fileList[selection];
            _asp.Play();
        
    }
    void _leftButtonPress()
    {
        if (selection > 0)
        {
            selection--;
        }
        else
        {
            selection = (byte)(fileList.Count -1);
        }
        Text = "SFX: " + selection.ToString("00");
    }
    void _rightButtonPress()
    {
        if (selection < fileList.Count -1)
        {
            selection++;
        }
        else
        {
            selection = 0;
        }
        Text = "SFX: " + selection.ToString("00");
    }

}
