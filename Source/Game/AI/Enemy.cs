using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Enemy Script.
/// </summary>
public class Enemy : Script
{
	[ShowInEditor, Serialize] private Behavior behavior;
	[ShowInEditor, Serialize] private float moveSpeed = 50f;
	private BasicKnowledge knowledge;

	[ShowInEditor, Serialize] private float gravity = -9.81f;

	[ShowInEditor] private State state;
	private CharacterController controller;
	/// <inheritdoc/>
	public override void OnStart()
	{
		controller = Actor.As<CharacterController>();
		behavior.StartLogic();
		knowledge = behavior.Knowledge.Blackboard as BasicKnowledge;
		knowledge.Player = SingletonManager.Get<Character>().Actor;
		knowledge.MoveSpeed = moveSpeed;
	}

	/// <inheritdoc/>
	public override void OnEnable()
	{
		// Here you can add code that needs to be called when script is enabled (eg. register for events)
	}

	/// <inheritdoc/>
	public override void OnDisable()
	{
		// Here you can add code that needs to be called when script is disabled (eg. unregister from events)
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		switch (state)
		{
			case State.Moving:

				break;

		}

	}

	public void ChangeState(State newState)
	{
		if (state == newState) return;
		state = newState;
	}

	public enum State
	{
		Idle,
		Moving,
		Attacking,
		Death
	}
}
