using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// DVial class.
/// </summary>
public class DVial
{
	public string Name;
	public Model Model;
	public Type VialType;
	public Prefab Prefab;

	public enum Type
	{
		Toxin,
		Health
	}
}
