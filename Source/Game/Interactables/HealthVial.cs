using FlaxEngine;

namespace Game;

public class HealthVial : Vial
{
	public override void Interact(Vector3 origin, Actor instigator)
	{
		// Heal the player
		var player = SingletonManager.Get<Character>();
		player.Heal(15);
		Destroy(Actor);
	}
}