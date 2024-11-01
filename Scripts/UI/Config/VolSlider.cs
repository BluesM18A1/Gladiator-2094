using Godot;
using System;

public partial class VolSlider : VSlider
{
    Config config;
    public enum AudioBuses {MASTER = 0, BGM = 1, SFX = 2};
    [Export]
    public AudioBuses bus;
    [Export]
    public Label DBlabel;
    public override void _Ready()
    {
        config = GetNode<Config>("/root/Config");
        config.Connect("RefreshUI",new Callable(this,"Refresh"), (uint)ConnectFlags.ReferenceCounted);
        DBlabel = GetNode<Label>("../dbLabel");
        Refresh();
    }

    void Refresh()
    {
        Value = AudioServer.GetBusVolumeDb((int)bus); 
        UpdateText();
    }
    void OnValueChange(float delta)
    {
        AudioServer.SetBusVolumeDb((int)bus, (float)Value); //db2linear or linear2db might be useful for volume controls
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
        dbLabelOperator = (Value > 0) ? "+" : "";
        DBlabel.Text = dbLabelOperator + InfOrNot() + " db";
    }
}
