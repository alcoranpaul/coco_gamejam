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
	[ShowInEditor, Serialize] private Prefab healthTriggerPrefab;
	[ShowInEditor, Serialize] private Actor healthTriggerSpawnPoint;

	private float timeElapsed = 0f;
	private float timeToSpawnHealthTrigger = 10f;
	private bool isThereCurrentlyHealthTrigger = false;
	private int treeIndex;
	private VialTrigger vialTrigger;

	public override void OnAwake()
	{
		if (treeActors == null)
			Debug.LogError("No trees found in the scene");

		treeIndex = 0;
	}

	public override void OnUpdate()
	{
		if (isThereCurrentlyHealthTrigger) return;

		timeElapsed += Time.DeltaTime;
		// Debug.Log($"Time elapsed: {timeElapsed}");
		if (timeElapsed >= timeToSpawnHealthTrigger)
		{

			timeElapsed = 0f;
			SpawnHealthTrigger();
		}
	}

	private void SpawnHealthTrigger()
	{
		var healthActor = PrefabManager.SpawnPrefab(healthTriggerPrefab, healthTriggerSpawnPoint.Position);
		if (healthActor.TryGetScript<VialTrigger>(out vialTrigger))
		{
			vialTrigger.OnVialCollected += OnVialCollected;
		}
		// Debug.Log("Health Trigger Spawned");
		isThereCurrentlyHealthTrigger = true;
	}

	private void OnVialCollected(object sender, EventArgs e)
	{
		vialTrigger.OnVialCollected -= OnVialCollected;
		vialTrigger = null;
		// Debug.Log("Health Trigger Collected");
		isThereCurrentlyHealthTrigger = false;
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

