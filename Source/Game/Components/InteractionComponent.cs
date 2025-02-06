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


	public InteractionComponent(InteractionArgs interactionArgs, Actor actor) : base()
	{

		this.actor = actor;
		_interactionArgs = interactionArgs;
		// _interactionArgs.TriggerColllider.TriggerEnter += OnTriggerEnter;
		SingletonManager.Get<InputManager>().OnCollectVial += CollectVial;

	}

	public static bool TryAssignInteract(IInteract obj)
	{
		var instance = SingletonManager.Get<InteractionComponent>();
		if (obj == null || instance == null || instance.objTointeract != null) return false;

		instance.objTointeract = obj;
		return true;
	}

	private void CollectVial()
	{
		if (objTointeract != null)
		{
			objTointeract.Interact(actor.Position, actor);
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

	}


}


