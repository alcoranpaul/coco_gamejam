using FlaxEngine;

namespace Game;

public class ToxinVial : Vial
{
	[ShowInEditor, Serialize] private Prefab throwablePrefab;
	[ShowInEditor, Serialize] private float throwForce = 3000f;
	[ShowInEditor, Serialize] private float throwUpForce = 500f;
	[ShowInEditor, Serialize] private float lifetime = 5f;

	public override void Interact(Vector3 origin, Actor instigator)
	{
		// Enable Throw
		Debug.Log($"Enable Throw");
		var throwActor = PrefabManager.SpawnPrefab(throwablePrefab, origin, instigator.Transform.Orientation);
		RigidBody rb = throwActor.As<RigidBody>();

		Vector3 force = instigator.Transform.Forward * throwForce + Transform.Up * throwUpForce;

		rb.AddForce(force, ForceMode.Impulse);

		Destroy(throwActor, lifetime);
		Destroy(Actor);
	}
}