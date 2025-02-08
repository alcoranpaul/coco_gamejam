using System;
using System.Collections.Generic;

using FlaxEngine;

namespace Game;

/// <summary>
/// InteractionComponent class.
/// </summary>
public class InteractionComponent : InstanceManagerClass
{

	private InteractionArgs _interactionArgs;
	private IInteract objTointeract;
	private Actor actor;

	public static event Action<bool> OnInteract;


	public InteractionComponent(InteractionArgs interactionArgs, Actor actor) : base()
	{

		this.actor = actor;
		_interactionArgs = interactionArgs;
		// _interactionArgs.TriggerColllider.TriggerEnter += OnTriggerEnter;
		SingletonManager.Get<InputManager>().OnCollectVial += CollectVial;

	}

	public static bool TryAssignInteract(IInteract obj, InventoryComponent.VialEquipped vialEquipped)
	{
		var instance = SingletonManager.Get<InteractionComponent>();
		var inventoryComponent = SingletonManager.Get<InventoryComponent>();
		if (obj == null || instance == null || instance.objTointeract != null || !inventoryComponent.CanHarvest(vialEquipped))
		{
			instance.objTointeract = null;
			OnInteract?.Invoke(false);
			return false;

		}

		instance.objTointeract = obj;
		OnInteract?.Invoke(true);
		return true;
	}

	private void CollectVial()
	{
		if (objTointeract != null)
		{
			objTointeract.Interact(actor.Position, actor);
			objTointeract = null;

			int randomIndex = Random.Shared.Next(0, _interactionArgs.harvestClips.Length);
			AudioClip harvestClip = _interactionArgs.harvestClips[randomIndex];
			SingletonManager.Get<SFXManager>().PlayAudio(harvestClip, actor.Position);
			OnInteract?.Invoke(false);
		}
	}

	public void OnDisable()
	{
		// _interactionArgs.TriggerColllider.TriggerEnter -= OnTriggerEnter;
		SingletonManager.Get<InputManager>().OnCollectVial -= CollectVial;
	}
	// private void OnTriggerEnter(PhysicsColliderActor actor)
	// {
	// 	if (Util.Layer.GetLayer(actor.Layer) != _interactionArgs.LayersMask || !actor.TryGetScript<IInteract>(out objTointeract)) return;

	// }

	public class InteractionArgs
	{
		public Collider TriggerColllider;
		public LayersMask LayersMask;
		public AudioClip[] harvestClips;

	}


}


