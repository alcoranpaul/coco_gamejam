using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// InventoryComponent interface.
/// </summary>
public class InventoryComponent : InstanceManagerClass
{

	private Vial toxinVial;
	private Vial healthVial;
	private InventoryArgs _inventoryArgs;
	public InventoryComponent(InventoryArgs inventoryArgs) : base()
	{
		_inventoryArgs = inventoryArgs;
	}

	public void AddVial(DVial vial)
	{
		if (vial.VialType == DVial.Type.Toxin)
		{
			Debug.Log("Toxin Vial added to Inventory");
		}
		else if (vial.VialType == DVial.Type.Health)
		{
			Debug.Log("Health Vial added to Inventory");
		}

		PrefabManager.SpawnPrefab(vial.Prefab, _inventoryArgs.ActorToAttach);
	}

	public class InventoryArgs
	{
		public Actor ActorToAttach;
	}
}
