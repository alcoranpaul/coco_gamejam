﻿using System;
using System.Collections.Generic;
using FlaxEngine;


namespace Game;

/// <summary>
/// Enemy Script.
/// </summary>
public class Enemy : Script, IDeath
{
	[ShowInEditor, Serialize] private Behavior behavior;
	[ShowInEditor, Serialize] private float moveSpeed = 50f;
	[ShowInEditor, Serialize] private float gravity = -9.81f;
	private BasicKnowledge knowledge;

	[ShowInEditor, Serialize] private AnimatedModel model;
	private AnimGraphParameter speedParam;
	private AnimGraphParameter isAttackingParam;
	private AnimGraphParameter isDeathParam;
	public float MoveSpeed => (float)speedParam.Value;

	[ShowInEditor] private State state;
	private CharacterController controller;

	[ShowInEditor, Serialize] private Collider attackTrigger;
	private bool isAttacking = false;
	public bool IsAttacking => isAttacking;
	private Tag attakTag = Tags.Find("Enemy.Attacking");



	/// <inheritdoc/>
	public override void OnStart()
	{
		controller = Actor.As<CharacterController>();
		speedParam = model.GetParameter("moveSpeed");
		isAttackingParam = model.GetParameter("isAttack");
		isDeathParam = model.GetParameter("isDeath");
		behavior.StartLogic();

		knowledge = behavior.Knowledge.Blackboard as BasicKnowledge;
		// knowledge.Agent = Actor.Parent;
		knowledge.Player = SingletonManager.Get<Character>().Actor;
		knowledge.MoveSpeed = moveSpeed;

		attackTrigger.IsActive = false;
		attackTrigger.TriggerEnter += OnTriggerEnter;


	}

	private void OnTriggerEnter(PhysicsColliderActor actor)
	{
		Debug.Log($"Enemy trigger enter: {actor}");
	}


	private void UpdateStateParameters(float speed, bool isAttacking, bool attackTriggerActive)
	{
		speedParam.Value = speed;
		isAttackingParam.Value = isAttacking;
		attackTrigger.IsActive = attackTriggerActive;
	}

	public override void OnUpdate()
	{
		switch (state)
		{
			case State.Moving:
				UpdateStateParameters(1f, false, false);
				break;
			case State.Idle:
				UpdateStateParameters(0f, false, false);

				if (controller.Velocity.LengthSquared > 0)
					ChangeState(State.Moving);
				break;
			case State.Death:

				UpdateStateParameters(0f, false, false);
				break;
			case State.Attacking:
				isAttacking = true;
				Actor.AddTag(attakTag);
				UpdateStateParameters(0f, true, true);
				break;
		}

	}



	public void ChangeState(State newState)
	{
		if (state == newState) return;
		state = newState;
		Debug.Log($"Enemy state changed to: {state}");
	}

	public void EndAttack()
	{
		isAttacking = false;
		Actor.RemoveTag(attakTag);
		ChangeState(State.Idle);


	}

	public void Die()
	{
		isDeathParam.Value = true;
		ChangeState(State.Death);
		behavior.StopLogic();
		behavior.Enabled = false;
		Actor.Layer = 4;
		controller.IsTrigger = true;
		Enabled = false;
		Destroy(Actor, 5f);
	}

	public enum State
	{
		Idle,
		Moving,
		Attacking,
		Death
	}
}


public interface IDeath
{
	void Die();
}