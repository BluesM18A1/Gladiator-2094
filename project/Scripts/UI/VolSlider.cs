using Godot;
using System;

public class VolSlider : VSlider
{
    public enum AudioBuses {MASTER = 0, BGM = 1, SFX = 2};
    [Export]
    public AudioBuses bus;
    [Export]
    public NodePath sndPath;
    public AudioStreamPlayer bloop;
    public Label DBlabel;
    public override void _Ready()
    {
        DBlabel = GetNode<Label>("../dbLabel/Label");
        bloop = GetNode<AudioStreamPlayer>(sndPath);
        Value = AudioServer.GetBusVolumeDb((int)bus); 
        UpdateText();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    void OnValueChange(float delta)
    {
        AudioServer.SetBusVolumeDb((int)bus, (float)Value); //db2linear or linear2db might be useful for volume controls
        bloop.Play();
        UpdateText();
        
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
