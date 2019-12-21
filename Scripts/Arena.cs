using Godot;
using System;

public class Arena : Spatial
{
    int score = 0;
    public Label topText;
    public Navigation nav;
    [Export]
    public float spawnRate = 20f, time = 0;
    [Export]
    public PackedScene Enemy = (PackedScene)ResourceLoader.Load("res://Prefabs/Enemy.tscn");
    [Export]
    public PackedScene HealthPack = (PackedScene)ResourceLoader.Load("res://Prefabs/Box_Health.tscn");
    [Export]
    public PackedScene AmmoBox = (PackedScene)ResourceLoader.Load("res://Prefabs/Box_Ammo.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        topText = GetNode<Label>("TopText");
        nav = GetNode<Navigation>("Navigation");
        ItemSpawn();
        topText.Text = "SCORE: " + score.ToString();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (time >= spawnRate)
        {
            EnemySpawn();
            ItemSpawn();
            time = 0;
        }
        else time += delta;
    }
    void EnemySpawn()
    {
        for (byte i = 0; i < 8 /*+ difficultyModifier */; i++)
        {
            RandomGroundSpawn(Enemy);
        }
    }
    void ItemSpawn()
    {
        for (byte i = 0; i < 4 /*- difficultyModifier */; i++)
        {
            int randomItem = (Int16)GD.RandRange(0,2); //A note about RandRange: the minimum value is INCLUSIVE and the max value is EXCLUSIVE
            switch (randomItem)
            {
                case (0):
                RandomGroundSpawn(AmmoBox);
                break;
                case (1):
                RandomGroundSpawn(HealthPack);
                break;
            }
        }
    }
    void RandomGroundSpawn(PackedScene item) //place object randomly within navmesh bounds
    {
        Spatial newItem = (Spatial)item.Instance();
        Vector3 randomPos = new Vector3((float)GD.RandRange(-18, 18), 2,(float)GD.RandRange(-18, 18));
        AddChild(newItem);
        newItem.Translation = nav.GetClosestPoint(randomPos);
    }
    public void UpdateScore(int delta)
    {
        score += delta;
        topText.Text = "SCORE: " + score.ToString();
    }
}
