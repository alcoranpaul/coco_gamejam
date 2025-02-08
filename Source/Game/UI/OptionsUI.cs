using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// OptionsUI Script.
/// </summary>
public class OptionsUI : Script
{
	private InGameSettings _inGameSettings;
	[ShowInEditor, Serialize] private UIControl mouseSensitivityControl;
	[ShowInEditor, Serialize] private UIControl bgmControl;
	[ShowInEditor, Serialize] private UIControl sfxControl;
	[ShowInEditor, Serialize] private AudioSource bgmSource;

	private Slider mouseSensitivitySlider;
	private Slider bgmSlider;
	private Slider sfxSlider;

	/// <inheritdoc/>
	public override void OnAwake()
	{
		JsonAsset asset = Engine.GetCustomSettings("InGameSettings") ?? throw new InvalidOperationException("InGameSettings asset not found.");

		_inGameSettings = asset.GetInstance<InGameSettings>();

		if (_inGameSettings == null)
		{
			throw new InvalidOperationException("InGameSettings asset not found.");
		}

		mouseSensitivitySlider = mouseSensitivityControl.Get<Slider>();
		bgmSlider = bgmControl.Get<Slider>();
		sfxSlider = sfxControl.Get<Slider>();
		bgmSource.Volume = _inGameSettings.MusicVolume;
	}

	private void OnBGMChanged()
	{
		_inGameSettings.MusicVolume = bgmSlider.Value;
		bgmSlider.Value = _inGameSettings.MusicVolume;
		bgmSource.Volume = _inGameSettings.MusicVolume;
	}


	private void OnSFXChanged()
	{
		_inGameSettings.SFXVolume = sfxSlider.Value;
		sfxSlider.Value = _inGameSettings.SFXVolume;
	}

	private void OnMouseSensitivityChanged()
	{
		_inGameSettings.MouseSentivity = mouseSensitivitySlider.Value;
		mouseSensitivitySlider.Value = _inGameSettings.MouseSentivity;
	}

	/// <inheritdoc/>
	public override void OnEnable()
	{
		JsonAsset asset = Engine.GetCustomSettings("InGameSettings") ?? throw new InvalidOperationException("InGameSettings asset not found.");

		_inGameSettings = asset.GetInstance<InGameSettings>();

		if (_inGameSettings == null)
		{
			throw new InvalidOperationException("InGameSettings asset not found.");
		}

		mouseSensitivitySlider.Value = _inGameSettings.MouseSentivity;
		bgmSlider.Value = _inGameSettings.MusicVolume;
		sfxSlider.Value = _inGameSettings.SFXVolume;


		mouseSensitivitySlider.ValueChanged += OnMouseSensitivityChanged;
		bgmSlider.ValueChanged += OnBGMChanged;
		sfxSlider.ValueChanged += OnSFXChanged;
	}

	public override void OnDisable()
	{
		mouseSensitivitySlider.ValueChanged -= OnMouseSensitivityChanged;
		bgmSlider.ValueChanged -= OnBGMChanged;
		sfxSlider.ValueChanged -= OnSFXChanged;
		base.OnDisable();
	}

	/// <inheritdoc/>
	public override void OnUpdate()
	{
		// Here you can add code that needs to be called every frame
	}
}
