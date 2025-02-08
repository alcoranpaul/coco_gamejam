using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Holdable Script.
/// </summary>
public abstract class Vial : Script, IInteract, IVial
{
	[ShowInEditor, Serialize] protected InventoryComponent.VialEquipped vialEquipped;

	public InventoryComponent.VialEquipped VialEquipped => vialEquipped;
	public event Action<InventoryComponent.VialEquipped> OnUsed;
	public abstract void Interact(Vector3 origin, Actor instigator);
	public void CallOnUsed()
	{
		OnUsed?.Invoke(vialEquipped);
	}

}

public interface IVial
{
	public InventoryComponent.VialEquipped VialEquipped { get; }
}