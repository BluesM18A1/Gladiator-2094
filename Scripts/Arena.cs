using Godot;
using System;

public partial class Arena : Node3D
{
	[Export]
	public bool debugMode;
	Config config;
	[Export]
	public byte MaxSubWaves = 3;
	int score = 0;
	int randomItem;
	byte wave, subwave;
	Rid map;
	public Label topText;
	public Control pauseMenu;
	AnimationPlayer dj;
	public AudioStreamPlayer maestro, announcer, crowd;
	[Export]
	public AudioStreamWav an_waveComplete = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/countdown20sec.wav")
	, an_go = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/GO-1.wav");
	public AudioStreamWav level = (AudioStreamWav)ResourceLoader.Load("res://Sounds/music/pathetique_weapons.wav"), boss  = (AudioStreamWav)ResourceLoader.Load("res://Sounds/music/pestered_archfiend.wav");
	[Export]
	public double spawnRate = 20f, time = 0;
	[Export((PropertyHint) 13)]
	public PackedScene[] EnemyTier1;
	[Export((PropertyHint) 13)]
	public PackedScene[] EnemyTier2;
	[Export((PropertyHint) 13)]
	public PackedScene[] BossTier;

	//I would just put all enemy tiers into an array of its own, but Godot does not support exporting custom data types to the editor yet.
	[Export((PropertyHint) 13)]
	public PackedScene[] ItemBoxes;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetWorld3D().NavigationMap;
		dj = GetNode<AnimationPlayer>("DJ");
		pauseMenu = GetNode<Control>("PauseScreen");
		config = GetNode<Config>("/root/Config");
		maestro = GetNode<AudioStreamPlayer>("Maestro");
		crowd = GetNode<AudioStreamPlayer>("Crowd");
		announcer = GetNode<AudioStreamPlayer>("Announcer");
		topText = GetNode<Label>("TextureRect/TopText");
		wave = config.startWave;
		UpdateScore(0);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!debugMode) ProcessGame(delta);
		
			
	}
	/*
	public override void _Input(InputEvent @event){
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
			{
				if (Input.MouseMode == Input.MouseModeEnum.Captured)
				{
					
				}
			}
		}
	}
	*/
	void ProcessGame(double delta)
	{
		if (subwave < MaxSubWaves)//note that the counter will go above 'maxsubwaves' by one before resetting
		{
			if (time >= spawnRate)
			{
				if (wave % 4 == 0 && wave > 1 && subwave == 0)
				{
					maestro.Stream = boss;
					maestro.Play();
					dj.Play("SongStart");
				}
				else if (subwave == 0 && wave > 1)
				{
					announcer.Stream = an_go;
					announcer.Play();
					maestro.Stream = level;
					maestro.Play(21.474f);
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
		if (wave % 4 == 0 && wave > 1 && subwave == 0)
		{
			randomItem = (Int16)GD.RandRange(0,BossTier.Length);
			announcer.Stream = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/boss_" + randomItem.ToString() + ".wav");
			announcer.Play();
			RandomGroundSpawn(BossTier[randomItem]);
			ItemSpawn();
		}
		else
		{
			switch (wave)
			{
				case 1:
					for (byte i = 0; i < 14 + (config.difficulty * 2); i++)
					{
						randomItem = (Int16)GD.RandRange(0,EnemyTier1.Length);
						RandomGroundSpawn(EnemyTier1[randomItem]);
					}
				break;
				default:
					for (byte i = 0; i < 10 + (config.difficulty * 2); i++)
					{
						randomItem = (Int16)GD.RandRange(0,EnemyTier1.Length);
						RandomGroundSpawn(EnemyTier1[randomItem]);
					}
					for (byte i = 0; i < 4 + (config.difficulty); i++)
					{
						randomItem = (Int16)GD.RandRange(0,EnemyTier2.Length);
						RandomGroundSpawn(EnemyTier2[randomItem]);
					}
				break;
			}
		}
		
		
	}
	void ItemSpawn()
	{
		for (byte i = 0; i < 8 - (config.difficulty * 2); i++)
		{
			//A note about RandfRange:
			// the minimum value is INCLUSIVE 
			//and the max value is EXCLUSIVE
			int randomItem = (Int16)GD.RandRange(0,ItemBoxes.Length); 
			RandomGroundSpawn(ItemBoxes[randomItem]);
		}
	}
	void RandomGroundSpawn(PackedScene item) //place object randomly within navmesh bounds
	{
		Node3D newItem = (Node3D)item.Instantiate();
		Vector3 randomPos = new Vector3((float)GD.RandRange(-28, 28), 2,(float)GD.RandRange(-28, 28));
		AddChild(newItem);
		//newItem.Position = nav.GetClosestPoint(randomPos);
		newItem.Position = NavigationServer3D.MapGetClosestPoint(map, randomPos);
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
