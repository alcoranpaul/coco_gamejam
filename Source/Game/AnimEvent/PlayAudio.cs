using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PlayAudio class.
/// </summary>
public class PlayAudio : AnimEvent
{
	public AudioClip[] audioClips;

	public override void OnEvent(AnimatedModel actor, Animation anim, float time, float deltaTime)
	{
		if (audioClips == null || audioClips.Length == 0)
		{
			return;
		}
		int randomIndex = Random.Shared.Next(0, audioClips.Length);
		AudioClip clip = audioClips[randomIndex];


		SingletonManager.Get<SFXManager>()?.PlayAudio(clip, actor.Position);
	}
}
