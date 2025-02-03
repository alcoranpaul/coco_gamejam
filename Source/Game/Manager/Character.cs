using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Character Script.
/// </summary>
public class Character : Script
{
	[ShowInEditor, Serialize]
	private MovementComponent.MovementArgs _movementArgs;
	[ShowInEditor, Serialize]
	private MovementComponent.CameraArgs _cameraArgs;
	[ShowInEditor, Serialize]
	private InteractionComponent.InteractionArgs _interactionArgs;
	[ShowInEditor, Serialize]
	private InventoryComponent.InventoryArgs _inventoryArgs;
	[ShowInEditor, Serialize]
	private ToxinComponent.ToxinArgs _toxinArgs;

	[ShowInEditor, Serialize] private float startingHealth = 100f;

	public HealthComponent HealthComponent { get; private set; }
	public ToxinComponent ToxinComponent { get; private set; }
	private MovementComponent _movementComponent;
	private InteractionComponent _interactionComponent;
	private InventoryComponent _inventoryComponent;
	private ThrowComponent _throwComponent;



	public override void OnAwake()
	{
		_movementComponent = new MovementComponent(_movementArgs, _cameraArgs, Actor.As<CharacterController>(), _movementArgs.CharacterObj.As<AnimatedModel>());
		_interactionComponent = new InteractionComponent(_interactionArgs);
		_inventoryComponent = new InventoryComponent(_inventoryArgs, _movementArgs.CharacterObj);
		_throwComponent = new ThrowComponent();

		HealthComponent = new HealthComponent(startingHealth);
		ToxinComponent = new ToxinComponent(_toxinArgs, healthComponent: HealthComponent);

		_inventoryComponent.OnToxinVialAdded += OnToxinVialAdded;
		_inventoryComponent.OnToxinVialRemoved += OnToxinVialRemoved;
		_throwComponent.OnThrowEnabled += OnThrowEnabled;
	}


	public override void OnDisable()
	{
		_interactionComponent.OnDisable();
		_throwComponent.OnDisable();
		_inventoryComponent.OnDisable();
		_inventoryComponent.OnToxinVialAdded -= OnToxinVialAdded;
		_inventoryComponent.OnToxinVialRemoved -= OnToxinVialRemoved;
		_throwComponent.OnThrowEnabled -= OnThrowEnabled;
		base.OnDisable();
	}

	public override void OnStart()
	{
		Screen.CursorLock = CursorLockMode.Locked;
		Screen.CursorVisible = false;

		_movementComponent.Start();
		HealthComponent.Start();
		ToxinComponent.Start();
	}

	public override void OnUpdate()
	{
		ToxinComponent.Update();
	}

	public override void OnFixedUpdate()
	{
		_movementComponent.Update();
	}

	private void OnThrowEnabled(object sender, EventArgs e)
	{
		Debug.Log($"Throw has been raised");
	}

	private void OnToxinVialAdded(bool flag)
	{
		ToxinComponent.Toxify(flag);
	}


	private void OnToxinVialRemoved(object sender, EventArgs e)
	{
		ToxinComponent.Toxify(false);
	}
}
