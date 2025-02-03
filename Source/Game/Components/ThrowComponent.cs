using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// ThrowComponent class.
/// </summary>
public class ThrowComponent : InstanceManagerClass
{
	public event EventHandler OnThrowEnabled;
	private State _state;
	public ThrowComponent() : base()
	{
		_state = State.Idle;
	}

	public void OnDisable()
	{

	}

	public static void EnableThrow()
	{
		if (SingletonManager.Get<ThrowComponent>()._state == State.Throwing) return;
		// Enable Throw
		Debug.Log($"Enable Throw -- ThrowComponent");
		SingletonManager.Get<ThrowComponent>()._state = State.Throwing;
		SingletonManager.Get<ThrowComponent>().OnThrowEnabled?.Invoke(SingletonManager.Get<ThrowComponent>(), EventArgs.Empty);

	}

	private enum State
	{
		Idle,
		Throwing
	}

}
