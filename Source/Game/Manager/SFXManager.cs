using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// SFXManager Script.
/// </summary>
public class SFXManager : InstanceManagerScript
{
	private InGameSettings _inGameSettings;
	/// <inheritdoc/>
	public override void OnStart()
	{
		JsonAsset asset = Engine.GetCustomSettings("InGameSettings") ?? throw new InvalidOperationException("InGameSettings asset not found.");

		_inGameSettings = asset.GetInstance<InGameSettings>();

		if (_inGameSettings == null)
		{
			throw new InvalidOperationException("InGameSettings asset not found.");
		}
	}


	public void PlayAudio(AudioClip clip, Vector3 position)
	{
		AudioSource source = new AudioSource();
		source.Clip = clip;
		source.PlayOnStart = true;
		source.Position = position;
		source.AllowSpatialization = true;
		source.Volume = _inGameSettings.SFXVolume;

		source.MinDistance = 30f;
		Level.SpawnActor(source);
		Destroy(source, clip.Length);

	}


}
