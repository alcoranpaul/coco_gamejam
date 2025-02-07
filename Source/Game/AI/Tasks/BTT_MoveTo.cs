using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// BTT_MoveTo Script.
/// </summary>
public class BTT_MoveTo : BehaviorTreeMoveToNode
{

	private Enemy enemy;
	public override void InitState(BehaviorUpdateContext context)
	{
		base.InitState(context);

		if (!context.Behavior.Actor.TryGetScript<Enemy>(out enemy))
			Debug.LogError("Enemy script not found!");

		enemy.RequestToMove();
	}

	public override void ReleaseState(BehaviorUpdateContext context)
	{
		enemy.ChangeState(Enemy.State.Idle);
		base.ReleaseState(context);

	}

	/// <inheritdoc />
	/// <inheritdoc />
	public override bool Move(Actor agent, Vector3 move)
	{

		// Ignore the vertical (Y) component of the move direction
		Vector3 flattenedMove = new Vector3(move.X, 0, move.Z).Normalized;

		// Ensure there is a valid direction to look at
		if (flattenedMove.LengthSquared > 0)
		{
			// Create a rotation to look in the flattened direction
			agent.Orientation = Quaternion.LookRotation(flattenedMove, Vector3.Up);
		}

		move.Y = Mathf.Abs(move.Y) * -9.81f;

		bool retVal = base.Move(agent, move);

		if (!retVal && enemy != null)
			enemy.ChangeState(Enemy.State.Moving);

		else

			enemy.ChangeState(Enemy.State.Idle);

		return retVal;
	}
}
