using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// AE_AttackEnd Script.
/// </summary>
public class AE_AttackEnd : AnimEvent
{
	public override void OnEvent(AnimatedModel actor, Animation anim, float time, float deltaTime)
	{
		Enemy script = null;
		Actor parent = actor.Parent;
		while (script == null)
		{
			if (!parent.TryGetScript<Enemy>(out script))
			{
				parent = parent.Parent;
			}

		}

		script.EndAttack();


	}
}
