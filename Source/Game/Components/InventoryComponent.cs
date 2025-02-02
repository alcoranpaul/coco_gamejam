using System;
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
	}

	public void AddVial(DVial vial)
	{
		if (vial.VialType == DVial.Type.Toxin)
		{
			Debug.Log("Toxin Vial added to Inventory");
			_toxinVialImage.Color = toxinVialColorUse;
			var controlColor = _inventoryArgs.toxinVialControl.Control.BackgroundColor;
			controlColor.A = 1;
			_inventoryArgs.toxinVialControl.Control.BackgroundColor = controlColor;
			OnToxinVialAdded?.Invoke(true);
		}
		else if (vial.VialType == DVial.Type.Health)
		{
			Debug.Log("Health Vial added to Inventory");
			_healthVialImage.Color = healthVialColorUse;
			var controlColor = _inventoryArgs.healthVialControl.Control.BackgroundColor;
			controlColor.A = 1;
			_inventoryArgs.healthVialControl.Control.BackgroundColor = controlColor;
			OnHealthVialAdded?.Invoke(true);
		}

		PrefabManager.SpawnPrefab(vial.Prefab, _inventoryArgs.ActorToAttach);
	}

	public class InventoryArgs
	{
		public Actor ActorToAttach;
		public UIControl toxinVialControl;
		public UIControl healthVialControl;
	}
}
