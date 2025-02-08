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
	[ShowInEditor, Serialize] private Type type;
	[ShowInEditor, Serialize] private AudioClip[] impactClips;
	[ShowInEditor, Serialize] private AudioClip splashClip;

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


			// Normal: Enemies
			if (type == Type.Normal)
			{
				if (Physics.OverlapSphere(Actor.Position, 150f, out Collider[] actors))
					DebugDraw.DrawWireSphere(new BoundingSphere(Actor.Position, 150f), Color.Red, 1f);
				foreach (var actor in actors)
				{

					if (actor.TryGetScript<IDeath>(out var death))
					{
						death.Die();

					}

				}

			}
			// Special: Tree
			else
			{
				if (Physics.OverlapSphere(Actor.Position, 150f, out Collider[] actors))
					DebugDraw.DrawWireSphere(new BoundingSphere(Actor.Position, 150f), Color.Green, 1f);
				foreach (var actor in actors)
				{
					if (actor.HasTag("ShipTree") && actor.TryGetScript<Ship>(out var ship))
					{
						ship.DecreaseTrees();
						break;
					}
				}

			}
			int randomIndex = Random.Shared.Next(0, impactClips.Length);
			AudioClip bottleBreakClip = impactClips[randomIndex];
			SingletonManager.Get<SFXManager>().PlayAudio(bottleBreakClip, Actor.Position);
			SingletonManager.Get<SFXManager>().PlayAudio(splashClip, Actor.Position);
			Destroy(vfxActor, 1f);
		}
	}

	public enum Type
	{
		Normal,
		Special
	}
}
