using Godot;
using System;

public class Arena : Spatial
{
    [Export]
    public float spawnRate = 20f, time = 0;
    [Export]
    public PackedScene Enemy = (PackedScene)ResourceLoader.Load("res://Prefabs/Enemy.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        EnemySpawn();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (time >= spawnRate)
        {
            EnemySpawn();
            time = 0;
        }
        else time += delta;
    }
    void EnemySpawn()
    {
        for (int i = 0; i < 6 /*+ difficultyModifier */; i++)
        {
            RandomSpawn(Enemy);
        }
    }
    void ItemSpawn()
    {

    }
    void RandomSpawn(PackedScene item)
    {
        Spatial newItem = (Spatial)item.Instance();
        Vector3 randomPos = new Vector3((float)GD.RandRange(-15, 15), 10,(float)GD.RandRange(-15, 15));
        AddChild(newItem);
        newItem.Translation = randomPos;
    }
}
