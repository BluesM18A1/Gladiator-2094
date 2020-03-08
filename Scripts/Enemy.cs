using Godot;
using System;

public class Enemy : Combatant
{
	[Export]
	public PackedScene deathExplosion = (PackedScene)ResourceLoader.Load("res://Prefabs/EnemyExplosion.tscn");
	[Export]
	public int bounty = 10;
	public Arena arena;
	Navigation nav;
	Spatial player;
	public Vector3 playerNavPos;
	Vector3 pathPos;
	Vector3[] path;
	AnimationPlayer ani;

	public float minDistance = 6;
	int pathPoint = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		arena = GetTree().Root.GetNode<Arena>("Spatial");
		ani = GetNode<AnimationPlayer>("AnimationPlayer");
		nav = GetNode<Navigation>("../Navigation");
		player = GetNode<Spatial>("../Player");
		playerNavPos = player.Translation;
		path = nav.GetSimplePath(Translation, playerNavPos, true);
		pathPos = Translation;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		playerNavPos = nav.GetClosestPoint(player.Translation);
		ProcessInput(delta);
		SetPathPos(playerNavPos);
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
		Vector2 inputMovementVector = Vector2.Zero;
		
		dir = new Vector3();
		
		float distanceToPoint = Translation.DistanceTo(pathPos);
		float distanceToPlayer = Translation.DistanceTo(playerNavPos);
		
		if (distanceToPoint > 0.5f && distanceToPlayer > minDistance)
		{
			inputMovementVector = new Vector2 (0,1);
		}
		else inputMovementVector = Vector2.Zero;
		
		LookAt(new Vector3(pathPos.x, Translation.y, pathPos.z), Vector3.Up); //rotation is going to look very odd and twitchy if I don't find some way to interpolate it....
		
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
	public float GetDistanceToPlayer()
	{
		
		Vector3 diff = player.Translation - Translation;
		float f = Mathf.Sqrt((diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z)); //Godot has a Normalize function but no Magnitude function? why?
		//GD.Print("magnitude: " + f);
		return f;
	}
}
