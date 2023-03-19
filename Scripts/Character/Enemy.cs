using Godot;
using System;

public partial class Enemy : Combatant
{
	//GAMEPLAY VARIABLES----------------------------------------------------------
	[Export]
	public int bounty = 10; //how much score do I add when killed
	//TODO: combine pathmode and range threshold into a struct and export an array of said struct
	public enum PathMode {STALKER, WANDERER, DEFENDER}; //which way I find my next position to move towards
	[Export]
	public float rangeThreshold = 6;
	[Export]
	public PathMode  closeRangeMode, longRangeMode;
	public PathMode currentMode;	
	public Arena arena;
	Rid map;
	public Node3D player;
	Vector3 pathPos;
	public AnimationPlayer ani;
	NavigationAgent3D nav;
	[Export]
	public float pathUpdateRate = 1;
	double updateTimer = 0;
	int pathPoint = 0;
	[Export]
	bool activated = false; //to be turned true by spawning/startup animation
	//COMPONENT VARIABLES---------------------------------------------------------
	[Export]
	public PackedScene deathExplosion;
	PackedScene pointsTally = (PackedScene)ResourceLoader.Load("res://Objects/Effects/PointsTally.tscn");
	Vector2 inputMovementVector = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		map = GetWorld3D().NavigationMap;
		arena = GetTree().Root.GetNode<Arena>("Node3D");
		ani = GetNode<AnimationPlayer>("AnimationPlayer");
		map = GetWorld3D().NavigationMap;
		nav = GetNode<NavigationAgent3D>("NavigationAgent3D");
		player = GetNode<Node3D>("../Player");
		GetTarget();
		currentMode = longRangeMode;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (activated)
		{
			PathUpdateTimer(delta);
			ProcessMovement(delta);
			ProcessInput(delta);
		}
		//failsafe in case an enemy finds its way out of bounds
		if (GlobalPosition.Length() > 1000) UpdateHealth(-9999, "error");
	}
	protected void ProcessInput(double delta) //this is where all the AI happens
	{
		float distanceToPlayer = Position.DistanceTo(player.Position);
		
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
		
		//if (currentMode != PathMode.DEFENDER) LookAt(new Vector3(nav.GetNextPathPosition().X, Position.Y, nav.GetNextPathPosition().Z), Vector3.Up);
		
		inputMovementVector = new Vector2(
			Position.DirectionTo(nav.GetNextPathPosition()).X,
			Position.DirectionTo(nav.GetNextPathPosition()).Z
		);
		
		//inputMovementVector = inputMovementVector.Normalized();
		dir = new Vector3(inputMovementVector.X, 0, inputMovementVector.Y);
		
	}
	public override async void UpdateHealth(int delta, string tag)
	{
		HP += delta;
		if (delta > 0)
		{
			//this is here in case we ever consider something that heals enemies
		}
		else 
		{
			if (HP <= 0)
			{
				//ani.Stop();
				if (ani.HasAnimation("Death"))
				{
					if (ani.CurrentAnimation == "Death") return;
					ani.Play("Death");
					await ToSignal(ani, "animation_finished");
				}
				if (deathExplosion != null)
				{
					Node3D boom = (Node3D)deathExplosion.Instantiate();
					boom.Position = Position;
					GetTree().CurrentScene.AddChild(boom);
				}
				if (tag == "Player1") //this check will need to be made more sophisticated when multiplayer comes back
				{
					arena.UpdateScore(bounty);
					Label3D points = (Label3D)pointsTally.Instantiate();
					GetTree().CurrentScene.AddChild(points);
					points.Text = "+"+bounty.ToString();
					points.GlobalPosition = GlobalPosition;
				}
				QueueFree();
			}
			else 
			{
				ani.Play("Hurt");	
			}
		}
		
	}
	public void GetTarget()
	{
		switch (currentMode)
		{
			case PathMode.STALKER:
			nav.TargetPosition = NavigationServer3D.MapGetClosestPoint(map, player.Position);
			break;
			case PathMode.WANDERER:
			nav.TargetPosition = NavigationServer3D.MapGetClosestPoint(map,new Vector3((float)GD.RandRange(-30, 30), 2,(float)GD.RandRange(-30, 30)));
			break;
			case PathMode.DEFENDER:
			nav.TargetPosition = NavigationServer3D.MapGetClosestPoint(map,Position);
			break;
			default:
			nav.TargetPosition = NavigationServer3D.MapGetClosestPoint(map,new Vector3((float)GD.RandRange(-30, 30), 2,(float)GD.RandRange(-30, 30)));
			break;
		}
	}
	public void PathUpdateTimer(double delta)
	{
		updateTimer += delta;
		if (updateTimer >= pathUpdateRate) 
		{
			GetTarget();
			updateTimer = 0;
		}
		//GD.Print(Position.DirectionTo(nav.GetNextPathPosition()));
			
	}
}
