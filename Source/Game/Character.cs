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



	private MovementComponent _movementComponent;

	public override void OnAwake()
	{
		_movementComponent = new MovementComponent(_movementArgs, _cameraArgs, Actor.As<CharacterController>(), _movementArgs.CharacterObj.As
		<AnimatedModel>());
	}

	public override void OnStart()
	{


		Screen.CursorLock = CursorLockMode.Locked;
		Screen.CursorVisible = false;

		_movementComponent.Start();
	}


	public override void OnFixedUpdate()
	{
		_movementComponent.Update();
	}
}

