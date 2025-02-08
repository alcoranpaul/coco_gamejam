using System;
using FlaxEngine;

namespace Game;

public class ToxinVial : Vial
{
	[ShowInEditor, Serialize] private Prefab throwablePrefab;
	[ShowInEditor, Serialize] private float throwForce = 3000f;
	[ShowInEditor, Serialize] private float throwUpForce = 500f;
	[ShowInEditor, Serialize] private float lifetime = 5f;


	private float throwMultiplier;
	private float maxThrowMultipler = 1500f;
	private bool isThrowing;
	private Vector3 origin;
	private Actor instigator;

	private float normalizedThrowMultiplier => throwMultiplier / maxThrowMultipler;


	public override void Interact(Vector3 origin, Actor instigator)
	{
		// Enable Throw
		SingletonManager.Get<InputManager>().OnMouseRelease += OnMouseRelease;
		isThrowing = true;
		this.origin = origin;
		this.instigator = instigator;

		throwMultiplier = 0f;
		ThrowUI.Instance.SetThrowing(true);
		Character.ToggleMovement(false);
	}

	public override void OnUpdate()
	{
		if (isThrowing && throwMultiplier <= maxThrowMultipler)
		{
			// Debug.Log($"Throwing: {throwMultiplier}");
			// Update Throw

			ThrowUI.Instance.SetValue(normalizedThrowMultiplier);
			throwMultiplier += Time.DeltaTime * 700f;
		}
	}

	public override void OnDisable()
	{
		SingletonManager.Get<InputManager>().OnMouseRelease -= OnMouseRelease;
		isThrowing = false;
		base.OnDisable();
	}

	public override void OnDestroy()
	{
		SingletonManager.Get<InputManager>().OnMouseRelease -= OnMouseRelease;
		isThrowing = false;
		base.OnDestroy();
	}

	private void OnMouseRelease()
	{
		// Debug.Log("OnMouseRelease");
		SingletonManager.Get<InputManager>().OnMouseRelease -= OnMouseRelease;
		isThrowing = false;


		var throwActor = PrefabManager.SpawnPrefab(throwablePrefab, origin, instigator.Transform.Orientation);


		Vector3 force = (instigator.Transform.Forward * throwForce + throwMultiplier) + Transform.Up * throwUpForce;
		RigidBody rb = throwActor.As<RigidBody>();
		// Debug.Log($"Force: {force}");
		rb.AddForce(force, ForceMode.Impulse);
		ThrowUI.Instance.SetThrowing(false);
		CallOnUsed();
		Character.ToggleMovement(true);
		Destroy(throwActor, lifetime);
		Destroy(Actor);
	}
}