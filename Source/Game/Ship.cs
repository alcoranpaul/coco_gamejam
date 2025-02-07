using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Ship Script.
/// </summary>
public class Ship : Script
{
	[ShowInEditor, Serialize] private Actor[] treeActors;
	private int treeIndex;

	public override void OnAwake()
	{
		if (treeActors == null)
			Debug.LogError("No trees found in the scene");

		treeIndex = 0;
	}


	public void DecreaseTrees()
	{
		treeActors[treeIndex].IsActive = false;
		treeIndex++;

		if (treeIndex >= treeActors.Length)
		{
			Debug.Log("All trees are destroyed");
			// WIN CONDITIOn
		}
	}
}

