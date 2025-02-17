﻿using System;
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
        Enemy script = null;
        Actor parent = actor.Parent;
        if (parent == null) return;
        while (script == null)
        {
            if (!parent.TryGetScript<Enemy>(out script))
            {
                parent = parent.Parent;
            }

        }

        if (script != null)
            script.Attack();


    }
}