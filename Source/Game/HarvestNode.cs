using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// HarvestNode Script.
/// </summary>
public class HarvestNode : Script, IInteract
{

	[ShowInEditor, Serialize] private Prefab prefabVialTrigger;
	[ShowInEditor, Serialize] private Collider collider;
	[ShowInEditor, Serialize] private InventoryComponent.VialEquipped vialEquipped;
	/// <inheritdoc/>
	public override void OnStart()
	{
		collider.IsTrigger = true;
		collider.TriggerEnter += TriggerEnter;
		collider.TriggerExit += TriggerExit;

		for (int i = 0; i < Actor.ChildrenCount - 1; i++)
		{
			Actor.GetChild(i).IsActive = false;
		}

		var childCount = Actor.ChildrenCount;
		var randomIndex = Random.Shared.Next(0, childCount - 1);
		Actor.GetChild(randomIndex).IsActive = true;
	}




	/// <inheritdoc/>
	public override void OnDisable()
	{
		collider.TriggerEnter -= TriggerEnter;
		collider.TriggerExit -= TriggerExit;
	}



	private void TriggerEnter(PhysicsColliderActor actor)
	{
		if (!actor.HasTag("Player")) return;

		InteractionComponent.TryAssignInteract(this, vialEquipped);
	}


	private void TriggerExit(PhysicsColliderActor actor)
	{
		if (!actor.HasTag("Player")) return;

		InteractionComponent.TryAssignInteract(null, vialEquipped);
	}

	public void Interact(Vector3 origin, Actor instigator)
	{

		PrefabManager.SpawnPrefab(prefabVialTrigger, origin);
		Destroy(Actor);
	}

}
