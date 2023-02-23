using Godot;
using System;

public partial class GameTimer : Label
{
    public float minutes, seconds, ms;
    public bool paused = false;
    [Export]
    AnimationPlayer blinky;
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ms += (float)delta * 100;
        if (ms > 99)
        {
            seconds++;
            ms = 0;
        }
        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }
        if (minutes > 59)
        {
            paused = true;
        }
        if (!paused) Text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + ms.ToString("00");
        else 
        {
            blinky.Play("blink");
        }
        
    }
}
