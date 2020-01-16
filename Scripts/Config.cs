using Godot;
using System;

public class Config : Node
{
    public int difficulty = 0;
    public int mouseSensitivity;
    public int score, highScore;
    public override void _Ready()
    {
        // we could load a config file from here maybe
    }
}
