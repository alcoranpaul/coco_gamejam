using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// InstructionUI Script.
/// </summary>
public class InstructionUI : Script
{
	[ShowInEditor, Serialize] private UIControl leftButtonControl;
	[ShowInEditor, Serialize] private UIControl rightButtonControl;
	[ShowInEditor, Serialize] private UIControl imageControl;
	[ShowInEditor, Serialize] private Texture[] images;

	private Button leftButton;
	private Button rightButton;
	private Image image;

	private int index;
	public override void OnStart()
	{
		leftButton = leftButtonControl.Get<Button>();
		rightButton = rightButtonControl.Get<Button>();

		image = imageControl.Get<Image>();
		image.Brush = new TextureBrush(images[index]);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		leftButton.Clicked += OnLeftButtonClicked;
		rightButton.Clicked += OnRightButtonClicked;
	}

	public override void OnDisable()
	{
		leftButton.Clicked -= OnLeftButtonClicked;
		rightButton.Clicked -= OnRightButtonClicked;
		base.OnDisable();
	}

	private void OnRightButtonClicked()
	{

		index++;
		if (index >= images.Length)
		{
			index = 0;
		}

		image.Brush = new TextureBrush(images[index]);
	}

	private void OnLeftButtonClicked()
	{

		index--;
		if (index < 0)
		{
			index = images.Length - 1;
		}

		image.Brush = new TextureBrush(images[index]);
	}

}
