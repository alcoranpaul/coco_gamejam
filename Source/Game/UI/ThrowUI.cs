using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// ThrowUI Script.
/// </summary>
public class ThrowUI : Script
{

	public static ThrowUI Instance { get; private set; }
	[ShowInEditor, Serialize] private UIControl throwUIControl;

	private ProgressBar throwBar;

	private bool isThrowing;

	public override void OnAwake()
	{
		if (Instance != null)
		{
			Debug.LogError("ThrowUI Instance already exists");
			return;
		}
		Instance = this;
	}

	public override void OnStart()
	{
		throwBar = throwUIControl.Get<ProgressBar>();
		throwBar.Value = 0f;
	}

	public override void OnUpdate()
	{
		if (!isThrowing && throwBar.Value > 0)
		{
			throwBar.Value -= Time.DeltaTime * 2f;
			// Decrease the progress bar
		}

	}

	public void SetThrowing(bool isThrowing)
	{
		this.isThrowing = isThrowing;

	}

	public void SetValue(float normalizedValue)
	{
		throwBar.Value = normalizedValue;

	}

}
