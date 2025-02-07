using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Utilities;

namespace Game;

/// <summary>
/// MainMenuUI Script.
/// </summary>
public class MainMenuUI : Script
{
	[ShowInEditor, Serialize] private UIControl playControl;
	[ShowInEditor, Serialize] private UIControl quitControl;
	[ShowInEditor, Serialize] private SceneReference gameScene;
	[ShowInEditor, Serialize] private UIControl continueControl;
	[ShowInEditor, Serialize] private UIControl instructionsControl;
	[ShowInEditor, Serialize] private UIControl[] controlToDisable;

	private Button playBtn;
	private Button quitBtn;
	private Button continueBtn;

	/// <inheritdoc/>
	public override void OnStart()
	{
		instructionsControl.IsActive = false;
		playBtn = playControl.Get<Button>();
		playBtn.Clicked += OnPlayClicked;
		quitBtn = quitControl.Get<Button>();
		quitBtn.Clicked += () => Engine.RequestExit();

		continueBtn = continueControl.Get<Button>();
		continueBtn.Clicked += () => Level.ChangeSceneAsync(gameScene);
	}

	private void OnPlayClicked()
	{
		instructionsControl.IsActive = true;
		controlToDisable.ForEach(c => c.Control.Enabled = false);
	}

	/// <inheritdoc/>
	public override void OnDisable()
	{
		playBtn.Clicked -= OnPlayClicked;
		quitBtn.Clicked -= () => Engine.RequestExit();
		continueBtn.Clicked -= () => Level.ChangeSceneAsync(gameScene);
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}
}
