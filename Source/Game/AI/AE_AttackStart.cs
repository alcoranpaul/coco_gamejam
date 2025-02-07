using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// AE_AttackStart Script.
/// </summary>
public class AE_AttackStart : AnimEvent
{
    public override void OnEvent(AnimatedModel actor, Animation anim, float time, float deltaTime)
    {
        if (actor.TryGetScript<Enemy>(out var enemy))
        {
            // enemy.ChangeState(Enemy.State.Idle);
        }
    }
}