using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// AudioSettings class.
/// </summary>
public class InGameSettings
{
	[Range(0, 1)]
	public float MouseSentivity = 0.5f;

	[Range(0, 1)]
	public float MusicVolume = 0.5f;

	[Range(0, 1)]
	public float SFXVolume = 0.5f;
}
