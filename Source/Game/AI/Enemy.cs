using System;
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

		controller.CollisionEnter += OnCollisionEnter;

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.OtherActor.HasTag("Enemy"))
		{
			Debug.Log($"Enemy {Actor.Name} collision with: {collision.OtherActor.Name}");
			// Vector3 pushDirection = collision.Contacts[0].Normal;
			// pushDirection.Y = 0f;
			// collision.OtherActor.As<CharacterController>().Move(pushDirection * 500f * Time.DeltaTime);

		}
	}

	public override void OnDisable()
	{
		controller.CollisionEnter -= OnCollisionEnter;
		attackTrigger.TriggerEnter -= OnTriggerEnter;
		base.OnDisable();
	}

	public override void OnDestroy()
	{
		controller.CollisionEnter -= OnCollisionEnter;
		attackTrigger.TriggerEnter -= OnTriggerEnter;
		base.OnDestroy();
	}

	private void OnTriggerEnter(PhysicsColliderActor actor)
	{
		if (!actor.TryGetScript<IDamage>(out var damageScript)) return;
		damageScript.TakeDamage(10f);

	}


	private void UpdateStateParameters(float speed, bool isAttacking, bool attackTriggerActive)
	{
		speedParam.Value = speed;
		isAttackingParam.Value = isAttacking;

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
				Actor.AddTag(tag: attakTag);
				UpdateStateParameters(0f, true, false);
				break;
		}

	}



	public void ChangeState(State newState)
	{
		if (state == newState) return;
		state = newState;
		// Debug.Log($"Enemy state changed to: {state}");
	}

	public void EndAttack()
	{
		isAttacking = false;
		Actor.RemoveTag(attakTag);
		attackTrigger.IsActive = false;
		ChangeState(State.Idle);

	}

	public void Attack()
	{

		attackTrigger.IsActive = true;
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
		attackTrigger.TriggerEnter -= OnTriggerEnter;
		controller.CollisionEnter -= OnCollisionEnter;
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