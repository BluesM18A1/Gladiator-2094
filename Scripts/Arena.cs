using Godot;
using System;

public class Arena : Spatial
{
	[Export]
	public bool debugMode;
	Config config;
	[Export]
	public byte MaxSubWaves = 3;
	int score = 0;
	int randomItem;
	byte wave, subwave;
	public Label topText;
	public Navigation nav;
	public Control pauseMenu;
	AnimationPlayer dj;
	public AudioStreamPlayer maestro, announcer, crowd;
	[Export]
	public AudioStreamSample an_waveComplete = (AudioStreamSample)ResourceLoader.Load("res://Sounds/announcer/countdown20sec.wav")
	, an_go = (AudioStreamSample)ResourceLoader.Load("res://Sounds/announcer/GO-1.wav");
	[Export]
	public float spawnRate = 20f, time = 0;
	public PackedScene Siren = (PackedScene)ResourceLoader.Load("res://Prefabs/Enemies/Siren.tscn");
	public PackedScene Cat = (PackedScene)ResourceLoader.Load("res://Prefabs/Enemies/Cat.tscn");
	[Export]
	public PackedScene HealthPack = (PackedScene)ResourceLoader.Load("res://Prefabs/Boxes/Box_Health.tscn");
	[Export]
	public PackedScene AmmoRepeater = (PackedScene)ResourceLoader.Load("res://Prefabs/Boxes/Box_Repeater.tscn");
	[Export]
	public PackedScene AmmoShells = (PackedScene)ResourceLoader.Load("res://Prefabs/Boxes/Box_Shells.tscn");
	[Export]
	public PackedScene AmmoGrenades = (PackedScene)ResourceLoader.Load("res://Prefabs/Boxes/Box_Grenades.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dj = GetNode<AnimationPlayer>("DJ");
		pauseMenu = GetNode<Control>("PauseScreen");
		config = GetNode<Config>("/root/Config");
		maestro = GetNode<AudioStreamPlayer>("Maestro");
		crowd = GetNode<AudioStreamPlayer>("Crowd");
		announcer = GetNode<AudioStreamPlayer>("Announcer");
		topText = GetNode<Label>("TextureRect/TopText");
		nav = GetNode<Navigation>("Navigation");
		wave = 1;
		UpdateScore(0);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!debugMode) ProcessGame(delta);
		
			
	}

	public override void _Input(InputEvent @event){
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
			{
				if (Input.GetMouseMode() == Input.MouseMode.Captured)
				{
					
				}
			}
		}
	}
	void ProcessGame(float delta)
	{
		if (subwave < MaxSubWaves)//note that the counter will go above 'maxsubwaves' by one before resetting to the en
		{
			if (time >= spawnRate)
			{
				if (subwave == 0 && wave > 1)
				{
					announcer.Stream = an_go;
					announcer.Play();
					dj.Play("SongStart");
				}
				EnemySpawn();
				ItemSpawn();
				subwave++;
				UpdateScore(0);
				time = 0;
			}
			else time += delta;
		}
		else if(!GetTree().HasGroup("Enemies"))
		{
			dj.Play("SongStop");
			announcer.Stream = an_waveComplete;
			announcer.Play();
			crowd.Play();
			score += wave * 100;
			wave++;
			subwave = 0;
			time = 0;
			UpdateScore(0);
			ItemSpawn();
		}
	}
	void EnemySpawn()
	{
		for (byte i = 0; i < 14 + (config.difficulty * 3); i++)
		{
			randomItem = (Int16)GD.RandRange(0,2);
			switch (randomItem)
			{
				default:
				RandomGroundSpawn(Siren);
				break;
				case (1):
				RandomGroundSpawn(Cat);
				break;
			}
		}
	}
	void ItemSpawn()
	{
		for (byte i = 0; i < 6 - (config.difficulty * 2); i++)
		{
			int randomItem = (Int16)GD.RandRange(0,5); //A note about RandRange:
			// the minimum value is INCLUSIVE 
			//and the max value is EXCLUSIVE
			switch (randomItem)
			{
				default:
				RandomGroundSpawn(HealthPack);
				break;
				case (1):
				RandomGroundSpawn(AmmoGrenades);
				break;
				case (2):
				RandomGroundSpawn(AmmoShells);
				break;
				case (3):
				RandomGroundSpawn(AmmoRepeater);
				break;
			}
		}
	}
	void RandomGroundSpawn(PackedScene item) //place object randomly within navmesh bounds
	{
		Spatial newItem = (Spatial)item.Instance();
		Vector3 randomPos = new Vector3((float)GD.RandRange(-28, 28), 2,(float)GD.RandRange(-28, 28));
		AddChild(newItem);
		newItem.Translation = nav.GetClosestPoint(randomPos);
		
	}
	public void UpdateScore(int delta)
	{
		if (debugMode)
		{
			topText.Text = "DEBUG MODE";
		}
		else
		{
			score += delta;
			topText.Text = "WAVE: " + wave.ToString()+"." + subwave.ToString() +"        " + "SCORE: " + score.ToString("000,000");
		}
	}
}
