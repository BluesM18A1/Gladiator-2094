using Godot;
using System;

public partial class Arena : Node3D
{
	[Export]
	public bool debugMode, gameOver;
	Config config;
	[Export]
	public byte MaxSubWaves = 3;
	int score = 0;
	int randomItem;
	byte wave, subwave;
	Rid map;
	public Label topTimer, waveCounter, scoreCounter;
	AnimationPlayer dj;
	public AudioStreamPlayer maestro, announcer, crowd;
	[Export]
	public AudioStreamWav an_waveComplete = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/countdown20sec.wav")
	, an_go = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/GO-1.wav"),
	an_gameover = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/gameover.wav");
	public AudioStreamWav level = (AudioStreamWav)ResourceLoader.Load("res://Sounds/music/level1.wav"), boss  = (AudioStreamWav)ResourceLoader.Load("res://Sounds/music/boss1.wav");
	[Export]
	public double spawnRate = 20f, time = 20;
	[Export((PropertyHint) 13)]
	public PackedScene[] EnemyTier1;
	[Export((PropertyHint) 13)]
	public PackedScene[] EnemyTier2;
	[Export((PropertyHint) 13)]
	public PackedScene[] BossTier;

	//TODO: put all enemy tiers into an array of its own
	[Export((PropertyHint) 13)]
	public PackedScene[] ItemBoxes;
	PackedScene player = (PackedScene)ResourceLoader.Load("res://Objects/Player.tscn");
	Godot.Collections.Array<Player> players = new Godot.Collections.Array<Player>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetWorld3D().NavigationMap;
		dj = GetNode<AnimationPlayer>("DJ");
		config = GetNode<Config>("/root/Config");
		maestro = GetNode<AudioStreamPlayer>("Maestro");
		crowd = GetNode<AudioStreamPlayer>("Crowd");
		announcer = GetNode<AudioStreamPlayer>("Announcer");
		topTimer = GetNode<Label>("TextureRect/Timer");
		waveCounter = GetNode<Label>("TextureRect/WaveCounter");
		scoreCounter = GetNode<Label>("TextureRect/ScoreCounter");
		AddPlayer();
		wave = config.startWave;
		UpdateScore(0);
	}
	void OnPlayerDeath()
	{
		GD.Print("player down");
		int i = 0;
		foreach (Player player in players)
		{
			if (!player.alive) i++;
		}
		if (i >= players.Count)
		{
			gameOver = true;
			announcer.Stream = an_gameover;
			announcer.Play();
			dj.Play("SongStop");
			foreach (Node enemy in GetTree().GetNodesInGroup("Enemies"))
			{
				enemy.QueueFree();
			}
		}
	}
	void AddPlayer()
	{
		//we are adding players to the scene by script so that they can be easily referenced by a list. Will be important for multiplayer when we start working on that.
		Player newPlayer = (Player)player.Instantiate();
		Vector3 randomPos = new Vector3((float)GD.RandRange(-28, 28), 10,(float)GD.RandRange(-28, 28));
		newPlayer.Position = NavigationServer3D.MapGetClosestPoint(map, randomPos);
		AddChild(newPlayer);
		players.Add(newPlayer);
		newPlayer.PlayerDeath += () => OnPlayerDeath();
		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!debugMode && !gameOver) ProcessGame(delta);
		waveCounter.Text = "WAVE: " + wave.ToString()+"." + subwave.ToString();
		DrawTimer();
		if (gameOver && Input.IsActionJustPressed("ui_cancel")) GetTree().ChangeSceneToFile("res://Title.tscn");
	}
	void DrawTimer()
	{
		float minutes = (float) time / 60;
		float ms = (float)time * 100;

		topTimer.Text = minutes.ToString("00") + ":" + ms.ToString("00:00");
	}
	void ProcessGame(double delta)
	{
		if (subwave < MaxSubWaves)//note that the counter will go above 'maxsubwaves' by one before resetting
		{
			if (time <= 0)
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
				time = spawnRate;
			}
			else time -= delta;
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
			time = spawnRate;
			UpdateScore(0);
			ItemSpawn();
		}
	}
	void EnemySpawn()
	{
		if (wave % 4 == 0 && wave > 1 && subwave == 0)
		{
			randomItem = (Int16)GD.RandRange(0,BossTier.Length -1);
			announcer.Stream = (AudioStreamWav)ResourceLoader.Load("res://Sounds/announcer/boss." + randomItem.ToString() + ".wav");
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
						randomItem = (Int16)GD.RandRange(0,EnemyTier1.Length-1);
						RandomGroundSpawn(EnemyTier1[randomItem]);
					}
				break;
				default:
					for (byte i = 0; i < 10 + (config.difficulty * 2); i++)
					{
						randomItem = (Int16)GD.RandRange(0,EnemyTier1.Length-1);
						RandomGroundSpawn(EnemyTier1[randomItem]);
					}
					for (byte i = 0; i < 4 + (config.difficulty); i++)
					{
						randomItem = (Int16)GD.RandRange(0,EnemyTier2.Length-1);
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
			int randomItem = (Int16)GD.RandRange(0,ItemBoxes.Length-1); 
			RandomGroundSpawn(ItemBoxes[randomItem]);
		}
	}
	void RandomGroundSpawn(PackedScene item) //place object randomly within navmesh bounds
	{
		Node3D newItem = (Node3D)item.Instantiate();
		Vector3 randomPos = new Vector3((float)GD.RandRange(-28, 28), 10,(float)GD.RandRange(-28, 28));
		newItem.Position = NavigationServer3D.MapGetClosestPoint(map, randomPos);
		AddChild(newItem);
		//newItem.Position = nav.GetClosestPoint(randomPos);
		
	}
	public void UpdateScore(int delta)
	{
		if (debugMode)
		{
			topTimer.Text = "";
			scoreCounter.Text = "";
			waveCounter.Text = "";
		}
		else
		{
			score += delta;
			scoreCounter.Text = "SCORE: " + score.ToString("000,000");
		}
	}
}
