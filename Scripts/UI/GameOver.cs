using Godot;
using System;

public partial class GameOver : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //TODO: move this code to the input method, no need to have anything on process.
  public override void _Process(double delta)
  {
      if (Input.IsActionJustPressed("ui_cancel"))
      {
          GetTree().ChangeSceneToFile("res://Title.tscn");
      }
  }
}
