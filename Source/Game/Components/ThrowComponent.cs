using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// ThrowComponent class.
/// </summary>
public class ThrowComponent
{
	public ThrowComponent()
	{
		SingletonManager.Get<InputManager>().OnUseVial += OnUseVial;
	}

	public void OnDisable()
	{
		SingletonManager.Get<InputManager>().OnUseVial -= OnUseVial;
	}

	private void OnUseVial()
	{
		Debug.Log($"ThrowComponent: OnUseVial");
		// Get the currently equipped vial
		// Call Interact on the Vial
	}

}
