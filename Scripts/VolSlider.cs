using Godot;
using System;

public class VolSlider : VSlider
{
    public enum AudioBuses {MASTER = 0, BGM = 1, SFX = 2};
    [Export]
    public AudioBuses bus;
    [Export]
    public NodePath lblPath;
    [Export]
    public NodePath sndPath;
    public AudioStreamPlayer bloop;
    public Label DBlabel;
    public override void _Ready()
    {
        //Value = AudioServer.GetBusVolumeDb((int)bus); why does this line crash the game when the db isn't 0?
        
        DBlabel = GetNode<Label>(lblPath);
        bloop = GetNode<AudioStreamPlayer>(sndPath);
        //UpdateText();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    void OnValueChange(float delta)
    {
        AudioServer.SetBusVolumeDb((int)bus, (float)Value);
        
        UpdateText();
        bloop.Play();
    }
    string InfOrNot()
    {
        string inf;
        if (Value <= -80){
            inf = "-Inf";
            return inf;
        }
        else return Value.ToString();
    }
    void UpdateText()
    {
        string dbLabelOperator;
        if (Value < 0)
        {
            dbLabelOperator = "";
        }
        else dbLabelOperator = "+";
        
        DBlabel.Text = dbLabelOperator + InfOrNot() + " db";
    }
}
