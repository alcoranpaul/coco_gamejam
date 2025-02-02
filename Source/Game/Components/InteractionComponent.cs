using System;
using System.Collections.Generic;

using FlaxEngine;

namespace Game;

/// <summary>
/// InteractionComponent class.
/// </summary>
public class InteractionComponent
{

	private InteractionArgs _interactionArgs;


	public InteractionComponent(InteractionArgs interactionArgs)
	{
		_interactionArgs = interactionArgs;
		_interactionArgs.TriggerColllider.TriggerEnter += OnTriggerEnter;


	}

	public void OnDisable()
	{
		_interactionArgs.TriggerColllider.TriggerEnter -= OnTriggerEnter;
	}
	private void OnTriggerEnter(PhysicsColliderActor actor)
	{
		if (Util.Layer.GetLayer(actor.Layer) == _interactionArgs.LayersMask)
		{
			Debug.Log("Interacted with object");
		}
	}

	public class InteractionArgs
	{
		public Collider TriggerColllider;
		public LayersMask LayersMask;

	}


}


