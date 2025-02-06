using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// InteractionUI Script.
/// </summary>
public class InteractionUI : Script
{
	[ShowInEditor, Serialize] private UIControl contentUI;

	/// <inheritdoc/>
	public override void OnStart()
	{
		InteractionComponent.OnInteract += OnInteract;
		contentUI.IsActive = false;
	}

	public override void OnDestroy()
	{
		InteractionComponent.OnInteract -= OnInteract;
		base.OnDestroy();
	}

	private void OnInteract(bool obj)
	{
		contentUI.IsActive = obj;
	}

}
