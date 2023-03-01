using Godot;
using System;

public partial class WaveChanger : Panel
{
    Config config;
    Label counter;
    byte startWave = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        counter = GetNode<Label>("Counter");
        UpdateCounter();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    void leftButtonDown()
    {
        if (startWave > 1)
        {
            startWave--;
            UpdateCounter();
        }
    }
    void rightButtonDown()
    {
        if (startWave < 5)
        {
            startWave++;
            UpdateCounter();
        }
    }
    void UpdateCounter()
    {
        config.startWave = startWave;
        counter.Text = startWave.ToString();
    }
}
