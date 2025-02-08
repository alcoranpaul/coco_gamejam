using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// WinOverUI Script.
/// </summary>
public class WinOverUI : Script
{
	[ShowInEditor, Serialize] private UIControl mainMenuControl;
	[ShowInEditor, Serialize] private UIControl quitControl;
	[ShowInEditor, Serialize] private SceneReference mainMenuScene;

	private Button mainMenuBtn;
	private Button quitBtn;
	/// <inheritdoc/>
	public override void OnStart()
	{
		Screen.CursorLock = CursorLockMode.Clipped;
		Screen.CursorVisible = true;

		mainMenuBtn = mainMenuControl.Get<Button>();
		mainMenuBtn.Clicked += OnMainMenuClicked;
		quitBtn = quitControl.Get<Button>();
		quitBtn.Clicked += () => Engine.RequestExit();
	}

	private void OnMainMenuClicked()
	{
		Level.ChangeSceneAsync(mainMenuScene);
	}

	/// <inheritdoc/>
	public override void OnEnable()
	{
		// Here you can add code that needs to be called when script is enabled (eg. register for events)
	}

	/// <inheritdoc/>
	public override void OnDisable()
	{
		mainMenuBtn.Clicked -= OnMainMenuClicked;
		quitBtn.Clicked -= () => Engine.RequestExit();
		// Here you can add code that needs to be called when script is disabled (eg. unregister from events)
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}
}
