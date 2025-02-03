using FlaxEngine;

namespace Game;

public class HealthVial : Vial
{
	public override void Interact(Vector3 origin, Actor instigator)
	{
		// Heal the player
		Debug.Log($"Heal the player");
		Destroy(Actor);
	}
}