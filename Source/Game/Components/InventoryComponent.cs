﻿using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// InventoryComponent interface.
/// </summary>
public class InventoryComponent : InstanceManagerClass
{
	public event Action<bool> OnToxinVialAdded;
	public event Action<bool> OnHealthVialAdded;

	private Vial toxinVial;
	private Vial healthVial;
	private InventoryArgs _inventoryArgs;


	private Image _toxinVialImage;
	private Image _healthVialImage;

	private Color toxinVialColorNone;
	private Color toxinVialColorUse;
	private Color healthVialColorNone;
	private Color healthVialColorUse;

	private VialEquipped _vialEquipped;

	public InventoryComponent(InventoryArgs inventoryArgs) : base()
	{
		_inventoryArgs = inventoryArgs;
		_toxinVialImage = _inventoryArgs.toxinVialControl.Get<Image>();
		_healthVialImage = _inventoryArgs.healthVialControl.Get<Image>();

		toxinVialColorNone = _toxinVialImage.Color;
		healthVialColorNone = _healthVialImage.Color;

		toxinVialColorUse = toxinVialColorNone;
		toxinVialColorUse.A = 1;
		healthVialColorUse = healthVialColorNone;
		healthVialColorUse.A = 1;

		_vialEquipped = VialEquipped.Toxin;
		SingletonManager.Get<InputManager>().OnUseVial += OnUseVial;
		SingletonManager.Get<InputManager>().OnSwapVial += OnSwapVial;
	}


	public void OnDisable()
	{
		SingletonManager.Get<InputManager>().OnUseVial -= OnUseVial;
		SingletonManager.Get<InputManager>().OnSwapVial -= OnSwapVial;
	}

	private void OnUseVial()
	{

	}

	private void OnSwapVial()
	{
		switch (_vialEquipped)
		{
			case VialEquipped.Toxin:
				if (healthVial == null) return;
				_vialEquipped = VialEquipped.Health;
				toxinVial.Actor.IsActive = false;
				healthVial.Actor.IsActive = true;
				break;
			case VialEquipped.Health:
				if (toxinVial == null) return;
				_vialEquipped = VialEquipped.Toxin;
				healthVial.Actor.IsActive = false;
				toxinVial.Actor.IsActive = true;
				break;

		}
	}


	public void AddVial(DVial vial)
	{
		// Determine the type of vial being added.
		bool isToxin = vial.VialType == DVial.Type.Toxin;
		bool isHealth = vial.VialType == DVial.Type.Health;

		// If the corresponding vial is already added, exit.
		if ((isToxin && toxinVial != null) || (isHealth && healthVial != null))
			return;

		// Check if this is the first vial added.
		bool isFirstVial = (toxinVial == null && healthVial == null);
		if (isFirstVial)
		{
			_vialEquipped = isToxin ? VialEquipped.Toxin : VialEquipped.Health;
		}

		// Update the UI and log message.
		if (isToxin)
		{
			Debug.Log("Toxin Vial added to Inventory");
			UpdateVialUI(_toxinVialImage, _inventoryArgs.toxinVialControl, toxinVialColorUse);
		}
		else if (isHealth)
		{
			Debug.Log("Health Vial added to Inventory");
			UpdateVialUI(_healthVialImage, _inventoryArgs.healthVialControl, healthVialColorUse);
		}

		// Spawn the vial actor.
		Actor vialActor = PrefabManager.SpawnPrefab(vial.Prefab, _inventoryArgs.ActorToAttach);

		// Assign the spawned actor's script to the appropriate variable and fire event.
		if (isToxin)
		{
			vialActor.TryGetScript<Vial>(out toxinVial);
			OnToxinVialAdded?.Invoke(true);
		}
		else if (isHealth)
		{
			vialActor.TryGetScript<Vial>(out healthVial);
			OnHealthVialAdded?.Invoke(true);
		}

		// Determine whether the spawned vial should be active.
		// If it's the first vial or if it matches the equipped type, set active; otherwise, inactive.
		bool isEquipped = isFirstVial || ((isToxin && _vialEquipped == VialEquipped.Toxin) || (isHealth && _vialEquipped == VialEquipped.Health));
		vialActor.IsActive = isEquipped;
	}

	private void UpdateVialUI(Image vialImage, UIControl vialControl, Color usedColor)
	{
		vialImage.Color = usedColor;
		var controlColor = vialControl.Control.BackgroundColor;
		controlColor.A = 1;
		vialControl.Control.BackgroundColor = controlColor;
	}


	public class InventoryArgs
	{
		public Actor ActorToAttach;
		public UIControl toxinVialControl;
		public UIControl healthVialControl;
	}

	private enum VialEquipped
	{
		Toxin,
		Health
	}
}
