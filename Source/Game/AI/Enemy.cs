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
	[ShowInEditor, Serialize] private float gravity = -9.81f;
	private BasicKnowledge knowledge;

	[ShowInEditor, Serialize] private AnimatedModel model;
	private AnimGraphParameter speedParam;
	private AnimGraphParameter isAttackingParam;
	public float MoveSpeed => (float)speedParam.Value;

	[ShowInEditor] private State state;
	private CharacterController controller;

	[ShowInEditor, Serialize] private Collider attackTrigger;
	private bool isAttacking = false;
	public bool IsAttacking => isAttacking;
	private Tag attakTag = Tags.Find("Enemy.Attacking");
	private bool requestMove = false;

	/// <inheritdoc/>
	public override void OnStart()
	{
		controller = Actor.As<CharacterController>();
		speedParam = model.GetParameter("moveSpeed");
		isAttackingParam = model.GetParameter("isAttack");

		behavior.StartLogic();

		knowledge = behavior.Knowledge.Blackboard as BasicKnowledge;
		knowledge.Agent = Actor;
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

	public void RequestToMove() => requestMove = true;

	public void ChangeState(State newState)
	{
		if (state == newState) return;
		state = newState;
	}

	public void EndAttack()
	{
		isAttacking = false;
		Actor.RemoveTag(attakTag);
		ChangeState(State.Idle);


	}

	public enum State
	{
		Idle,
		Moving,
		Attacking,
		Death
	}
}
