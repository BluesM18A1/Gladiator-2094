using Godot;
using System;
/*
A sabot is a device that allows a projectile of a smaller caliber to be fired from a weapon of a larger caliber by filling the weapon's bore and keeping the projectile centered. The sabot normally separates and falls away from the projectile a short distance from the muzzle. 

Similarly, this script is only here to help other objects spawn multiple nodes at once by way of a single packedscene, and once instantiated and put in the scene tree, all the children are unparented and this despawns.

If I put this node on a simple lifetime timer like its (normally as it is intended) bullet children, that could also work but I think it's kinda ugly. It feels more elegant to have it immediately unparent the children and despawn, leaving the children to have as long of a lifetime as they please.
*/

public partial class Sabot : Node3D
{
    [Signal]
    public delegate void SabotDeployedEventHandler();
    public override void _Ready()
	{
		foreach (Node node in GetChildren())
        {
            node.Reparent(GetParent());
        }
        QueueFree();
        EmitSignal(SignalName.SabotDeployed);
    }
}
