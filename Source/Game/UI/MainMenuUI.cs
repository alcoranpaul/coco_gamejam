using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// MainMenuUI Script.
/// </summary>
public class MainMenuUI : Script
{
	[ShowInEditor, Serialize] private UIControl playControl;
	[ShowInEditor, Serialize] private UIControl quitControl;
	[ShowInEditor, Serialize] private SceneReference gameScene;
	private Button playBtn;
	private Button quitBtn;

	/// <inheritdoc/>
	public override void OnStart()
	{
		playBtn = playControl.Get<Button>();
		playBtn.Clicked += OnPlayClicked;
		quitBtn = quitControl.Get<Button>();
		quitBtn.Clicked += () => Engine.RequestExit();
	}

	private void OnPlayClicked()
	{
		Level.ChangeSceneAsync(gameScene);
	}

	/// <inheritdoc/>
	public override void OnEnable()
	{
		// Here you can add code that needs to be called when script is enabled (eg. register for events)
	}

	/// <inheritdoc/>
	public override void OnDisable()
	{
		playBtn.Clicked -= OnPlayClicked;
		quitBtn.Clicked -= () => Engine.RequestExit();
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}
}
