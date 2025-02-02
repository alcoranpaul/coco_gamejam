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

	[ShowInEditor, Serialize] private float startingHealth = 100f;

	private MovementComponent _movementComponent;
	public HealthComponent HealthComponent { get; private set; }
	private InteractionComponent _interactionComponent;


	public override void OnAwake()
	{
		_movementComponent = new MovementComponent(_movementArgs, _cameraArgs, Actor.As<CharacterController>(), _movementArgs.CharacterObj.As
		<AnimatedModel>());
		_interactionComponent = new InteractionComponent(_interactionArgs);

		HealthComponent = new HealthComponent(startingHealth);
	}
	public override void OnDisable()
	{
		_interactionComponent.OnDisable();
		base.OnDisable();
	}

	public override void OnStart()
	{


		Screen.CursorLock = CursorLockMode.Locked;
		Screen.CursorVisible = false;

		_movementComponent.Start();
		HealthComponent.Start();
	}


	public override void OnFixedUpdate()
	{
		_movementComponent.Update();
	}
}

