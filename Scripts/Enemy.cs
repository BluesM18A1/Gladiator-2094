using Godot;
using System;

public partial class Enemy : Combatant
{
	//GAMEPLAY VARIABLES----------------------------------------------------------
	[Export]
	public int bounty = 10; //how much score do I add when killed
	public enum PathMode {STALKER, WANDERER, DEFENDER}; //which way I find my next position to move towards
	[Export]
	public float rangeThreshold = 6;
	[Export]
	public PathMode  closeRangeMode, longRangeMode;
	public PathMode currentMode;	
	public Arena arena;
	Rid map;
	public Node3D player;
	public Vector3 targetNavPos;
	Vector3 pathPos;
	Vector3[] path;
	AnimationPlayer ani;
	//NavigationAgent3D nav;
	[Export]
	public float pathUpdateRate = 1;
	double updateTimer = 0;
	int pathPoint = 0;
	//COMPONENT VARIABLES---------------------------------------------------------
	[Export]
	public PackedScene deathExplosion = (PackedScene)ResourceLoader.Load("res://Prefabs/EnemyExplosion.tscn");
	
	Vector2 inputMovementVector = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetWorld3D().NavigationMap;
		arena = GetTree().Root.GetNode<Arena>("Node3D");
		ani = GetNode<AnimationPlayer>("AnimationPlayer");
		map = GetWorld3D().NavigationMap;
		player = GetNode<Node3D>("../Player");
		targetNavPos = player.Position;
		path = NavigationServer3D.MapGetPath(map, Position, targetNavPos, true);
		pathPos = Position;
		currentMode = longRangeMode;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PathUpdateTimer(delta);
		GetTarget();
		SetPathPos(targetNavPos);
		ProcessInput(delta);
		if (HP <= 0)
		{
			CpuParticles3D boom = (CpuParticles3D)deathExplosion.Instantiate();
			GetTree().Root.AddChild(boom);
			boom.Emitting = true;
			boom.Position = Position;
			
			arena.UpdateScore(bounty);
			QueueFree();
		}
	}
	protected void ProcessInput(double delta) //this is where all the AI happens
	{
		Transform3D camXform = GlobalTransform;
		dir = new Vector3();
		
		float distanceToPoint = Position.DistanceTo(pathPos);
		float distanceToPlayer = Position.DistanceTo(NavigationServer3D.MapGetClosestPoint(map, player.Position));
		
		if (distanceToPlayer > rangeThreshold)
		{
			//GD.Print("outside range");
			currentMode = longRangeMode;
		}
		else
		{
			//GD.Print("within range");
			currentMode = closeRangeMode;
		}
		//the following line exists to rotate enemy wheels if it has any. I should find a way to interpolate the rotation to make the animation more natural.
		//if (currentMode != PathMode.DEFENDER) LookAt(new Vector3(pathPos.X, Position.Y, pathPos.Z), Vector3.Up);
		
		
		inputMovementVector = inputMovementVector.Normalized();
		dir += -camXform.Basis.Z.Normalized() * inputMovementVector.Y;
		dir += camXform.Basis.X.Normalized() * inputMovementVector.X;
		
		
	}
	public override void UpdateHealth(int delta)
	{
		HP += delta;
		if (delta > 0)
		{
			//this is here in case we ever consider something that heals enemies
		}
		else 
		{
			ani.Play("Hurt");//Add hit sound to this animation!
		}
		
	}
	void SetPathPos(Vector3 newPos) //This makes the movement vector follow the right point in the path array
	{
		
		pathPos = path[pathPoint];
		float distanceToPoint = Position.DistanceTo(pathPos);
		if (distanceToPoint < 0.5f)
		{
			if (path.Length == 0)
			{
				GD.Print("Cannot find Path3D!");
				return;
			}
			//GD.Print("Target Reached");
			if (pathPoint < path.Length -1) //if it isn't on the final path point
			{
				pathPoint++; //increment point counter
				pathPos = path[pathPoint]; //set the movement target to the next point
			}
			else 
			{
				UpdatePath(newPos); //get a new array of path points
			}
		}
	}
	public void UpdatePath(Vector3 newPos)
	{
		pathPoint = 0; //reset counter
		path = NavigationServer3D.MapGetPath(map, Position, newPos, true); //generate new array of points
	}
	public void GetTarget()
	{
		switch (currentMode)
		{
			case PathMode.STALKER:
			targetNavPos = NavigationServer3D.MapGetClosestPoint(map, player.Position);
			inputMovementVector = new Vector2 (0,1);
			break;
			case PathMode.WANDERER:
			targetNavPos = NavigationServer3D.MapGetClosestPoint(map,new Vector3((float)GD.RandRange(-30, 30), 2,(float)GD.RandRange(-30, 30)));
			inputMovementVector = new Vector2 (0,1);
			break;
			case PathMode.DEFENDER:
			targetNavPos = NavigationServer3D.MapGetClosestPoint(map,Position);
			inputMovementVector = new Vector2 (0,0);
			break;
			default:
			targetNavPos = NavigationServer3D.MapGetClosestPoint(map,new Vector3((float)GD.RandRange(-30, 30), 2,(float)GD.RandRange(-30, 30)));
			inputMovementVector = new Vector2 (0,1);
			break;
		}
	}
	public void PathUpdateTimer(double delta)
	{
		updateTimer += delta;
		if (updateTimer >= pathUpdateRate) 
		{
			UpdatePath(targetNavPos);
			updateTimer = 0;
		}
			
	}
	//
}
