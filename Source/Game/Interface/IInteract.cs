using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// IInteract interface.
/// </summary>
public interface IInteract
{
    public void Interact(Vector3 origin, Actor instigator);
}
