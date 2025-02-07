using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// BTT_Attack Script.
/// </summary>
public class BTT_Attack : BehaviorTreeNode
{
	private Enemy enemy;
	public override void InitState(BehaviorUpdateContext context)
	{
		if (!context.Behavior.Actor.TryGetScript<Enemy>(out enemy))
			Debug.LogError("Enemy script not found!");
		enemy.ChangeState(Enemy.State.Attacking);

	}



	public override BehaviorUpdateResult Update(BehaviorUpdateContext context)
	{
		return BehaviorUpdateResult.Success;
	}
}
