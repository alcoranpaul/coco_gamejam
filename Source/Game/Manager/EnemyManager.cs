using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// EnemyManager Script.
/// </summary>
public class EnemyManager : InstanceManagerScript
{
	[ShowInEditor, Serialize] private Prefab enemyPrefab;
	[ShowInEditor, Serialize] private int maxEnemies = 10;
	[ShowInEditor, Serialize] private Actor[] spawnPoints;
	public override void OnAwake()
	{
		base.OnAwake();
	}

}
