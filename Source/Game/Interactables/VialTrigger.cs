using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// VialTrigger Script.
/// </summary>
public class VialTrigger : Script
{
	[ShowInEditor, Serialize] private JsonAssetReference<DVial> VialType;
	[ShowInEditor, Serialize] private Collider collider;
	[ShowInEditor, Serialize] private Actor modelActor;
	public event EventHandler OnVialCollected;

	/// <inheritdoc/>
	public override void OnStart()
	{
		collider.IsTrigger = true;
		collider.TriggerEnter += TriggerEnter;
		PrefabManager.SpawnPrefab(VialType.Instance.Prefab, modelActor);

	}

	private void TriggerEnter(PhysicsColliderActor actor)
	{
		if (!actor.HasTag("Player")) return;

		// Send the Vial to the Inventory
		SingletonManager.Get<InventoryComponent>()?.AddVial(VialType.Instance);
		OnVialCollected?.Invoke(this, EventArgs.Empty);
		// Destroy Actor
		Destroy(Actor);
	}


	/// <inheritdoc/>
	public override void OnDisable()
	{
		collider.TriggerEnter -= TriggerEnter;
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Move the modelActor with Sin function
		float speed = 2.0f;
		float height = 0.5f;
		Vector3 position = modelActor.Position;
		position.Y += Mathf.Sin(Time.GameTime * speed) * height;
		modelActor.Position = position;
	}
}
