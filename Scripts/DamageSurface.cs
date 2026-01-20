using Godot;
using System;

public partial class DamageSurface : Area3D
{
	[Signal]
	public delegate void DealDamageEventHandler(int damagePoints);
	[Export]
	float damageRate = 1;
	[Export]
	int damageAmount = -15;
	double timer = 0;
	Godot.Collections.Array<Node> bodies = new Godot.Collections.Array<Node>();

	public override void _Process(double delta)
	{
		timer+= delta;
		if (timer > damageRate)
		{
			foreach (Node body in bodies)
			{
				EmitSignal(SignalName.DealDamage, damageAmount, GetGroups()[0].ToString());
			}
			timer = 0;
		}
	}
	private void _on_body_entered(Node body)
	{
		if (body.HasMethod("UpdateHealth"))
		{
			bodies.Add(body);
			if (IsConnected(SignalName.DealDamage,new Callable(body,"UpdateHealth")))	
				Connect(SignalName.DealDamage,new Callable(body,"UpdateHealth"), (uint)ConnectFlags.ReferenceCounted);
		}
	}
	private void _on_body_exited(Node body)
	{
		if (body.HasMethod("UpdateHealth"))
		{
			bodies.Remove(body);
			Disconnect(SignalName.DealDamage,new Callable(body,"UpdateHealth"));
		}
		
	}
}
