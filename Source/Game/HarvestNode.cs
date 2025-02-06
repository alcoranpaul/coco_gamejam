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
	/// <inheritdoc/>
	public override void OnStart()
	{
		collider.IsTrigger = true;
		collider.TriggerEnter += TriggerEnter;
		collider.TriggerExit += TriggerExit;
	}




	/// <inheritdoc/>
	public override void OnDisable()
	{
		collider.TriggerEnter -= TriggerEnter;
		collider.TriggerExit -= TriggerExit;
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}

	private void TriggerEnter(PhysicsColliderActor actor)
	{
		if (!actor.HasTag("Player")) return;

		InteractionComponent.TryAssignInteract(this);
	}


	private void TriggerExit(PhysicsColliderActor actor)
	{
		if (!actor.HasTag("Player")) return;

		InteractionComponent.TryAssignInteract(null);
	}

	public void Interact(Vector3 origin, Actor instigator)
	{
		Debug.Log($"Interacted with object at {origin} by {instigator}");
		PrefabManager.SpawnPrefab(prefabVialTrigger, origin);
		Destroy(Actor);
	}

}
