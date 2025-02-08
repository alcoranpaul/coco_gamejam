using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// GameOverUI Script.
/// </summary>
public class GameOverUI : Script
{
	[ShowInEditor, Serialize] private UIControl gameOverUIControl;
	[ShowInEditor, Serialize] private UIControl mainMenuControl;
	[ShowInEditor, Serialize] private UIControl quitControl;
	[ShowInEditor, Serialize] private SceneReference mainMenuScene;
	private Button mainMenuBtn;
	private Button quitBtn;

	/// <inheritdoc/>
	public override void OnStart()
	{
		mainMenuBtn = mainMenuControl.Get<Button>();
		mainMenuBtn.Clicked += OnMainMenuClicked;
		quitBtn = quitControl.Get<Button>();
		quitBtn.Clicked += () => Engine.RequestExit();
		SingletonManager.Get<Character>().OnDeathEvent += OnPlayerDeath;
		gameOverUIControl.IsActive = false;

		Screen.CursorLock = CursorLockMode.Clipped;
		Screen.CursorVisible = true;
	}

	public override void OnDisable()
	{
		mainMenuBtn.Clicked -= OnMainMenuClicked;
		quitBtn.Clicked -= () => Engine.RequestExit();
		SingletonManager.Get<Character>().OnDeathEvent -= OnPlayerDeath;
		base.OnDisable();
	}

	private void OnMainMenuClicked()
	{
		Level.ChangeSceneAsync(mainMenuScene);
	}

	private void OnPlayerDeath(object sender, EventArgs e)
	{
		gameOverUIControl.IsActive = true;
	}


}
