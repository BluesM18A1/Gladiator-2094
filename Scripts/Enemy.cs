using Godot;
using System;

public class Enemy : Combatant
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
	Navigation nav;
	Spatial player;
	public Vector3 targetNavPos;
	Vector3 pathPos;
	Vector3[] path;
	AnimationPlayer ani;
	
	int pathPoint = 0;
	//COMPONENT VARIABLES---------------------------------------------------------
	[Export]
	public PackedScene deathExplosion = (PackedScene)ResourceLoader.Load("res://Prefabs/EnemyExplosion.tscn");
	
	Vector2 inputMovementVector = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		arena = GetTree().Root.GetNode<Arena>("Spatial");
		ani = GetNode<AnimationPlayer>("AnimationPlayer");
		nav = GetNode<Navigation>("../Navigation");
		player = GetNode<Spatial>("../Player");
		targetNavPos = player.Translation;
		path = nav.GetSimplePath(Translation, targetNavPos, true);
		pathPos = Translation;
		currentMode = longRangeMode;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		GetTarget();
		SetPathPos(targetNavPos);
		ProcessInput(delta);
		if (HP <= 0)
		{
			CPUParticles boom = (CPUParticles)deathExplosion.Instance();
			GetTree().Root.AddChild(boom);
			boom.Emitting = true;
			boom.Translation = Translation;
			
			arena.UpdateScore(bounty);
			QueueFree();
		}
	}
	protected void ProcessInput(float delta) //this is where all the AI happens
	{
		Transform camXform = GlobalTransform;
		dir = new Vector3();
		
		float distanceToPoint = Translation.DistanceTo(pathPos);
		float distanceToPlayer = Translation.DistanceTo(nav.GetClosestPoint(player.Translation));
		
		if (distanceToPoint > 0.5f && distanceToPlayer > rangeThreshold)
		{
			//GD.Print("outside range");
			currentMode = longRangeMode;
		}
		else
		{
			//GD.Print("within range");
			currentMode = closeRangeMode;
		}
		
		if (currentMode != PathMode.DEFENDER) LookAt(new Vector3(pathPos.x, Translation.y, pathPos.z), Vector3.Up);
		//this line exists to rotate enemy wheels if it has any. I should find a way to interpolate the rotation to make the animation more natural.
		
		inputMovementVector = inputMovementVector.Normalized();
		dir += -camXform.basis.z.Normalized() * inputMovementVector.y;
		dir += camXform.basis.x.Normalized() * inputMovementVector.x;
		
		
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
	void SetPathPos(Vector3 newPos)
	{
		
		pathPos = path[pathPoint];
		float distanceToPoint = Translation.DistanceTo(pathPos);
		if (distanceToPoint < 0.5f)
		{
			if (path.Length == 0)
			{
				GD.Print("Cannot find Path!");
				return;
			}
			//GD.Print("Target Reached");
			if (pathPoint < path.Length -1)
			{
				//GD.Print("Changing direction");
				pathPoint++;
				pathPos = path[pathPoint];
				//GD.Print(pathPoint.ToString());
			}
			else 
			{
				UpdatePath(newPos);
			}
		}
		
	}
	public void UpdatePath(Vector3 newPos)
	{
		pathPoint = 0;
		path = nav.GetSimplePath(Translation, newPos, true);
	}
	public void GetTarget()
	{
		switch (currentMode)
		{
			case PathMode.STALKER:
			targetNavPos = nav.GetClosestPoint(player.Translation);
			inputMovementVector = new Vector2 (0,1);
			break;
			case PathMode.WANDERER:
			targetNavPos = nav.GetClosestPoint(new Vector3((float)GD.RandRange(-28, 28), 2,(float)GD.RandRange(-28, 28)));
			inputMovementVector = new Vector2 (0,1);
			break;
			case PathMode.DEFENDER:
			targetNavPos = nav.GetClosestPoint(Translation);
			inputMovementVector = new Vector2 (0,0);
			break;
			default:
			targetNavPos = nav.GetClosestPoint(new Vector3((float)GD.RandRange(-28, 28), 2,(float)GD.RandRange(-28, 28)));
			inputMovementVector = new Vector2 (0,1);
			break;
		}
	}
	public float GetDistanceToPlayer()
	{
		
		Vector3 diff = player.Translation - Translation;
		float f = Mathf.Sqrt((diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z)); //Godot has a Normalize function but no Magnitude function? why?
		//GD.Print("magnitude: " + f);
		return f;
	}
}
