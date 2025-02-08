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
	public event Action<bool, DVial.Toxin> OnToxinVialAdded;
	public event Action<bool> OnHealthVialAdded;



	public event Action<DVial.Toxin> OnToxinVialRemoved;
	public event EventHandler OnHealthVialRemoved;

	private Vial normalToxinVial;
	private Vial specialToxinVial;
	private Vial healthVial;

	private InventoryArgs _inventoryArgs;


	private Image _normalToxinVialImage;
	private Image _specialToxinVialImage;
	private Image _healthVialImage;

	private Color toxinVialColorNone;
	private Color toxinVialColorUse;
	private Color specialVialColorNone;
	private Color specialVialColorUse;
	private Color healthVialColorNone;
	private Color healthVialColorUse;

	private VialEquipped _vialEquipped;

	private Actor character;

	public InventoryComponent(InventoryArgs inventoryArgs, Actor character) : base()
	{
		this.character = character;
		_inventoryArgs = inventoryArgs;
		_normalToxinVialImage = _inventoryArgs.NormalToxinVialControl.Get<Image>();
		_healthVialImage = _inventoryArgs.HealthVialControl.Get<Image>();
		_specialToxinVialImage = _inventoryArgs.SpecialToxinVialControl.Get<Image>();


		toxinVialColorNone = _normalToxinVialImage.Color;
		healthVialColorNone = _healthVialImage.Color;
		specialVialColorNone = _specialToxinVialImage.Color;

		toxinVialColorUse = toxinVialColorNone;
		toxinVialColorUse.A = 1;

		specialVialColorUse = specialVialColorNone;
		specialVialColorUse.A = 1;

		healthVialColorUse = healthVialColorNone;
		healthVialColorUse.A = 1;

		_vialEquipped = VialEquipped.NToxin;
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
		switch (_vialEquipped)
		{
			case VialEquipped.NToxin:
				if (normalToxinVial == null) return;
				normalToxinVial.Interact(_inventoryArgs.ActorToAttach.Position, character);
				UpdateVialUI(_normalToxinVialImage, _inventoryArgs.NormalToxinVialControl, toxinVialColorNone, 0.5f);
				// FlaxEngine.Object.Destroy(normalToxinVial.Actor);

				break;
			case VialEquipped.SToxin:
				if (specialToxinVial == null) return;
				specialToxinVial.Interact(_inventoryArgs.ActorToAttach.Position, character);
				UpdateVialUI(_specialToxinVialImage, _inventoryArgs.SpecialToxinVialControl, toxinVialColorNone, 0.5f);
				// FlaxEngine.Object.Destroy(specialToxinVial.Actor);

				break;
			case VialEquipped.Health:
				if (healthVial == null) return;
				healthVial.Interact(_inventoryArgs.ActorToAttach.Position, character);
				UpdateVialUI(_healthVialImage, _inventoryArgs.HealthVialControl, healthVialColorNone, 0.5f);
				// FlaxEngine.Object.Destroy(healthVial.Actor);

				break;
		}
	}

	private void OnSwapVial()
	{

		if (healthVial == null && specialToxinVial == null && normalToxinVial == null) return;


		switch (_vialEquipped)
		{
			case VialEquipped.NToxin:
				if (healthVial != null)
				{
					EquipVial(VialEquipped.Health, healthVial, normalToxinVial, specialToxinVial);
				}
				else if (specialToxinVial != null)
				{
					EquipVial(VialEquipped.SToxin, specialToxinVial, normalToxinVial, healthVial);
				}
				break;
			case VialEquipped.SToxin:
				if (healthVial != null)
				{
					EquipVial(VialEquipped.Health, healthVial, normalToxinVial, specialToxinVial);
				}
				else if (normalToxinVial != null)
				{
					EquipVial(VialEquipped.NToxin, normalToxinVial, healthVial, specialToxinVial);
				}
				break;
			case VialEquipped.Health:
				if (specialToxinVial != null)
				{
					EquipVial(VialEquipped.SToxin, specialToxinVial, normalToxinVial, healthVial);
				}
				else if (normalToxinVial != null)
				{
					EquipVial(VialEquipped.NToxin, normalToxinVial, healthVial, specialToxinVial);
				}
				break;
		}

	}

	private void EquipVial(VialEquipped newVial, Vial activeVial, Vial inactiveVial1, Vial inactiveVial2)
	{
		int randomIndex = Random.Shared.Next(0, _inventoryArgs.vialSwapClips.Length);

		_vialEquipped = newVial;
		if (inactiveVial1 != null)
			inactiveVial1.Actor.IsActive = false;
		if (inactiveVial2 != null)
			inactiveVial2.Actor.IsActive = false;
		activeVial.Actor.IsActive = true;

		AudioClip vialSwapClip = _inventoryArgs.vialSwapClips[randomIndex];
		SingletonManager.Get<SFXManager>().PlayAudio(vialSwapClip, character.Position);
	}
	public void AddVial(DVial vial)
	{
		// Determine the type of vial being added.
		bool isToxin = vial.VialType == DVial.Type.Toxin;
		bool isHealth = vial.VialType == DVial.Type.Health;

		// If the corresponding vial is already added, exit.
		if ((isToxin && normalToxinVial != null && specialToxinVial != null) || (isHealth && healthVial != null))
		{
			Debug.Log("Vial already added to Inventory");
			return;

		}

		// Check if this is the first vial added.
		bool isFirstVial = normalToxinVial == null && healthVial == null && specialToxinVial == null;
		if (isFirstVial)
		{

			if (isToxin)
			{
				if (vial.ToxinType == DVial.Toxin.Normal)
					_vialEquipped = VialEquipped.NToxin;
				else if (vial.ToxinType == DVial.Toxin.Special)
					_vialEquipped = VialEquipped.SToxin;
			}
			else
				_vialEquipped = VialEquipped.Health;
		}

		// Update the UI and log message.
		if (isToxin)
		{
			// Debug.Log("Toxin Vial added to Inventory");
			if (vial.ToxinType == DVial.Toxin.Normal)
				UpdateVialUI(_normalToxinVialImage, _inventoryArgs.NormalToxinVialControl, toxinVialColorUse);
			else if (vial.ToxinType == DVial.Toxin.Special)
				UpdateVialUI(_specialToxinVialImage, _inventoryArgs.SpecialToxinVialControl, specialVialColorUse);
		}
		else if (isHealth)
		{
			// Debug.Log("Health Vial added to Inventory");
			UpdateVialUI(_healthVialImage, _inventoryArgs.HealthVialControl, healthVialColorUse);
		}

		// Spawn the vial actor.
		Actor vialActor = PrefabManager.SpawnPrefab(vial.Prefab, _inventoryArgs.ActorToAttach);

		// Assign the spawned actor's script to the appropriate variable and fire event.
		if (isToxin)
		{
			if (vial.ToxinType == DVial.Toxin.Normal)
			{
				vialActor.TryGetScript<Vial>(out normalToxinVial);
				normalToxinVial.OnUsed += OnNormalToxinUsed;
				OnToxinVialAdded?.Invoke(true, DVial.Toxin.Normal);
			}
			else
			{
				vialActor.TryGetScript<Vial>(out specialToxinVial);
				specialToxinVial.OnUsed += OnSpecialToxinUsed;
				OnToxinVialAdded?.Invoke(true, DVial.Toxin.Special);
			}

		}
		else if (isHealth)
		{
			vialActor.TryGetScript<Vial>(out healthVial);
			healthVial.OnUsed += OnHealthVialUsed;
			OnHealthVialAdded?.Invoke(true);
		}



		// Determine whether the spawned vial should be active.
		// If it's the first vial or if it matches the equipped type, set active; otherwise, inactive.
		bool shouldShow = isFirstVial ||
						  (isToxin && ((vial.ToxinType == DVial.Toxin.Normal && _vialEquipped == VialEquipped.NToxin) ||
									   (vial.ToxinType == DVial.Toxin.Special && _vialEquipped == VialEquipped.SToxin))) ||
						  (isHealth && _vialEquipped == VialEquipped.Health);
		vialActor.IsActive = shouldShow;

	}

	private void OnHealthVialUsed(VialEquipped equipped)
	{
		healthVial.OnUsed -= OnHealthVialUsed;
		healthVial = null;
		OnHealthVialRemoved?.Invoke(this, EventArgs.Empty);
	}

	private void OnSpecialToxinUsed(VialEquipped equipped)
	{
		specialToxinVial.OnUsed -= OnSpecialToxinUsed;
		specialToxinVial = null;
		OnToxinVialRemoved?.Invoke(DVial.Toxin.Special);
	}

	private void OnNormalToxinUsed(VialEquipped equipped)
	{
		normalToxinVial.OnUsed -= OnNormalToxinUsed;
		normalToxinVial = null;
		OnToxinVialRemoved?.Invoke(DVial.Toxin.Normal);
	}

	private void UpdateVialUI(Image vialImage, UIControl vialControl, Color usedColor, float alpha = 1)
	{
		vialImage.Color = usedColor;
		var controlColor = vialControl.Control.BackgroundColor;
		controlColor.A = alpha;
		vialControl.Control.BackgroundColor = controlColor;
	}


	public class InventoryArgs
	{
		public Actor ActorToAttach;
		public UIControl NormalToxinVialControl;
		public UIControl SpecialToxinVialControl;
		public UIControl HealthVialControl;
		public AudioClip[] vialSwapClips;
	}

	public enum VialEquipped
	{
		NToxin,
		SToxin,
		Health
	}
}
