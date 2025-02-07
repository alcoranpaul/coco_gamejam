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
	[ShowInEditor, Serialize] private SceneReference winScene;
	[ShowInEditor, Serialize] private Prefab treeDissolveVFX;
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
		Vector3 vfxPos = treeActors[treeIndex].Position;
		vfxPos.Y += 100f;
		var vfxActor = PrefabManager.SpawnPrefab(treeDissolveVFX, vfxPos);
		treeIndex++;
		Destroy(vfxActor, 2f);
		if (treeIndex >= treeActors.Length)
		{
			Debug.Log("All trees are destroyed");
			Level.ChangeSceneAsync(winScene);
			// WIN CONDITIOn
		}
	}
}

