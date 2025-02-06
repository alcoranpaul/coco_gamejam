using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Throwable Script.
/// </summary>
public class Throwable : Script
{
	[ShowInEditor, Serialize] private Collider collider;
	[ShowInEditor, Serialize] private Prefab vfxPrefab;

	private bool firstCollision = true;
	public override void OnStart()
	{
		collider.CollisionEnter += OnCollisionEnter;
	}

	public override void OnDisable()
	{
		collider.CollisionEnter -= OnCollisionEnter;
		base.OnDisable();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (firstCollision)
		{
			firstCollision = false;
			var vfxActor = PrefabManager.SpawnPrefab(vfxPrefab, Actor.Position);
			Destroy(Actor, 1f);
		}
	}

}
