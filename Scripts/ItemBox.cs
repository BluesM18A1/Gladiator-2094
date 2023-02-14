using Godot;
using System;

public partial class ItemBox : Area3D
{
	//GAMEPLAY VARIABLES-------------------------------------
	public enum ITEM {MEDKIT, BULLETS, SHELLS, GRENADES}
	[Export]
	ITEM itemType;
	[Export]
	public byte itemCount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	private void _OnCollisionEnter(Player body)
	{
		if (body.IsInGroup("Players"))
		{
			switch (itemType)
			{
				case ITEM.MEDKIT: //IF HEALTH
				body.UpdateHealth(itemCount);
				break;
				case ITEM.BULLETS:
				body.gun.AddBullets(itemCount);
				break;
				case ITEM.SHELLS:
				body.gun.AddShells(itemCount);
				break;
				case ITEM.GRENADES:
				body.gun.AddGrenades(itemCount);
				break;
			}
			QueueFree();
		}
		
	}
}
