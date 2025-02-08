using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// MenuOptions Script.
/// </summary>
public class MenuOptions : Script
{
	[ShowInEditor, Serialize] private UIControl loreControl;
	[ShowInEditor, Serialize] private UIControl objectiveControl;
	[ShowInEditor, Serialize] private UIControl optionsControl;
	[ShowInEditor, Serialize] private UIControl creditsControl;

	[ShowInEditor, Serialize] private UIControl loreContentControl;
	[ShowInEditor, Serialize] private UIControl objectiveContentControl;
	[ShowInEditor, Serialize] private UIControl optionsContentControl;
	[ShowInEditor, Serialize] private UIControl creditsContentControl;

	private Button loreButton;
	private Button objectiveButton;
	private Button optionsButton;
	private Button creditsButton;

	/// <inheritdoc/>
	public override void OnStart()
	{

		loreButton = loreControl.Get<Button>();
		objectiveButton = objectiveControl.Get<Button>();
		optionsButton = optionsControl.Get<Button>();
		creditsButton = creditsControl.Get<Button>();

		loreContentControl.IsActive = false;
		objectiveContentControl.IsActive = false;
		optionsContentControl.IsActive = false;
		creditsContentControl.IsActive = false;

		loreButton.Clicked += OnLoreClicked;
		objectiveButton.Clicked += OnObjectiveClicked;
		optionsButton.Clicked += OnOptionsClicked;
		creditsButton.Clicked += OnCreditsClicked;
	}

	private void OnCreditsClicked()
	{
		loreContentControl.IsActive = false;
		objectiveContentControl.IsActive = false;
		optionsContentControl.IsActive = false;
		creditsContentControl.IsActive = !creditsContentControl.IsActive;
	}

	private void OnOptionsClicked()
	{
		loreContentControl.IsActive = false;
		objectiveContentControl.IsActive = false;
		creditsContentControl.IsActive = false;
		optionsContentControl.IsActive = !optionsContentControl.IsActive;
	}


	private void OnObjectiveClicked()
	{
		loreContentControl.IsActive = false;
		objectiveContentControl.IsActive = !objectiveContentControl.IsActive;
		creditsContentControl.IsActive = false;
		optionsContentControl.IsActive = false;
	}

	private void OnLoreClicked()
	{
		loreContentControl.IsActive = !loreContentControl.IsActive;
		creditsContentControl.IsActive = false;
		objectiveContentControl.IsActive = false;
		optionsContentControl.IsActive = false;
	}


	/// <inheritdoc/>
	public override void OnDisable()
	{
		loreButton.Clicked -= OnLoreClicked;
		objectiveButton.Clicked -= OnObjectiveClicked;
		optionsButton.Clicked -= OnOptionsClicked;
		creditsButton.Clicked -= OnCreditsClicked;
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}
}
