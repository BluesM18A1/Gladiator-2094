using Godot;
using System;

public class Config : Node
{
    public int difficulty = 0;
    public float mouseSensitivity = 0.10f;
    public int score, highScore;
    public override void _Ready()
    {
        // we could load a config file from here maybe
    }
}
