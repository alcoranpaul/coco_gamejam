using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// MovementComponent class.
/// </summary>
public class MovementComponent
{

	private MovementArgs _movementArgs;
	private CameraArgs _cameraArgs;

	private AnimatedModel _model;
	private AnimGraphParameter _speedParam;
	private AnimGraphParameter _isSprintParam;

	private CharacterController _controller;
	private Vector3 _velocity;

	private float _yaw;
	private float _pitch;
	private float _defaultFov;

	public MovementComponent(MovementArgs movementArgs, CameraArgs cameraArgs, CharacterController controller, AnimatedModel model)
	{
		_movementArgs = movementArgs;
		_cameraArgs = cameraArgs;
		_controller = controller;
		_model = model;

		if (!_movementArgs.CharacterObj || !_cameraArgs.CameraView)
		{
			Debug.LogError("No Character or Camera assigned!");
			return;
		}

	}

	public void Start()
	{
		_defaultFov = _cameraArgs.CameraView.FieldOfView;
		_speedParam = _model.GetParameter("moveSpeed");
		_isSprintParam = _model.GetParameter("isSprint");
	}

	public void Update()
	{
		// Get input axes
		var inputH = 0f; // Input.GetAxis("Horizontal")
		var inputV = Input.GetAxis("Vertical");

		// Apply movement towards the camera direction
		var movement = new Vector3(inputH, 0.0f, inputV);
		// Camera Rotation
		{
			// Get mouse axis values and clamp pitch
			_yaw += Input.GetAxis("Mouse X") * _cameraArgs.MouseSensitivity * Time.DeltaTime; // H
			_pitch += Input.GetAxis("Mouse Y") * _cameraArgs.MouseSensitivity * Time.DeltaTime; // V
			_pitch = Mathf.Clamp(_pitch, _cameraArgs.PitchMinMax.X, _cameraArgs.PitchMinMax.Y);

			// The camera's parent should be another actor, like a spring arm for instance
			_cameraArgs.CameraView.Parent.Orientation = Quaternion.Lerp(_cameraArgs.CameraView.Parent.Orientation, Quaternion.Euler(_pitch, _yaw, 0), Time.DeltaTime * _cameraArgs.CameraLag);
			if (movement.LengthSquared > 0f)
				_movementArgs.CharacterObj.Orientation = Quaternion.Euler(0, _yaw, 0);

			// When right clicking, zoom in or out
			if (Input.GetAction("Aim"))
			{
				_cameraArgs.CameraView.FieldOfView = Mathf.Lerp(_cameraArgs.CameraView.FieldOfView, _cameraArgs.FOVZoom, Time.DeltaTime * 5f);
			}
			else
			{
				_cameraArgs.CameraView.FieldOfView = Mathf.Lerp(_cameraArgs.CameraView.FieldOfView, _defaultFov, Time.DeltaTime * 5f);
			}
		}

		// Character Movement
		{

			var movementDirection = _cameraArgs.CameraView.Transform.TransformDirection(movement);

			// Jump if the space bar is down, jump
			if (_controller.IsGrounded && Input.GetAction("Jump"))
			{
				_velocity.Y = Mathf.Sqrt(_movementArgs.JumpStrength * -2f * _movementArgs.Gravity);
			}

			// Apply gravity
			_velocity.Y += _movementArgs.Gravity * Time.DeltaTime;
			movementDirection += (_velocity * 0.5f);

			// Apply controller movement, evaluate whether we are sprinting or not
			var speed = (Input.GetAction("Sprint") ? _movementArgs.SprintSpeed : _movementArgs.Speed) * Time.DeltaTime;
			_controller.Move(movementDirection * speed);

			float moveSpeed = 0f;
			if (movement.LengthSquared > 0f)
			{
				moveSpeed = 1f;
			}


			_speedParam.Value = Input.GetAction("Sprint") ? 2f : moveSpeed;
			_isSprintParam.Value = Input.GetAction("Sprint");


		}
	}

	[Serializable]
	public class MovementArgs
	{
		// Groups names for UI
		private const string MovementGroup = "Movement";


		// Movement
		[ExpandGroups]
		[Tooltip("The character model"), EditorDisplay(MovementGroup, "Character"), EditorOrder(2)]
		public Actor CharacterObj { get; set; } = null;

		[Range(200f, 400f), Tooltip("Movement speed factor"), EditorDisplay(MovementGroup, "Speed"), EditorOrder(3)]
		public float Speed { get; set; } = 250;

		[Range(300f, 500f), Tooltip("Movement speed factor"), EditorDisplay(MovementGroup, "Sprint Speed"), EditorOrder(4)]
		public float SprintSpeed { get; set; } = 400;

		[Limit(-20f, 20f), Tooltip("Gravity of this character"), EditorDisplay(MovementGroup, "Gravity"), EditorOrder(5)]
		public float Gravity { get; set; } = -9.81f;

		[Range(0f, 25f), Tooltip("Jump factor"), EditorDisplay(MovementGroup, "Jump Strength"), EditorOrder(6)]
		public float JumpStrength { get; set; } = 10;

		[ShowInEditor, Serialize, EditorDisplay(MovementGroup, "Animated Model"), EditorOrder(7)]
		private AnimatedModel _model;
	}
	[Serializable]
	public class CameraArgs
	{
		private const string CameraGroup = "Camera";
		[Tooltip("The camera view for player"), EditorDisplay(CameraGroup, "Camera View"), EditorOrder(8)]
		public Camera CameraView { get; set; } = null;

		[Range(0, 10f), Tooltip("Sensitivity of the mouse"), EditorDisplay(CameraGroup, "Mouse Sensitivity"), EditorOrder(9)]
		public float MouseSensitivity { get; set; } = 100f;

		[Range(0f, 20f), Tooltip("Lag of the camera, lower = slower"), EditorDisplay(CameraGroup, "Camera Lag"), EditorOrder(10)]
		public float CameraLag { get; set; } = 10;

		[Range(0f, 100f), Tooltip("How far to zoom in, lower = closer"), EditorDisplay(CameraGroup, "FOV Zoom"), EditorOrder(11)]
		public float FOVZoom { get; set; } = 50;

		[Tooltip("Determines the min and max pitch value for the camera"), EditorDisplay(CameraGroup, "Pitch Min Max"), EditorOrder(12)]
		public Float2 PitchMinMax { get; set; } = new Float2(-45, 45);

	}
}
