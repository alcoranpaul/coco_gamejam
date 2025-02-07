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
	[ShowInEditor, Serialize] private int[] maxEnemies = [10, 15, 18, 20, 25];
	private int currentEnemiesToSpawn;
	private int enemiesToSpawnIndex = 0;

	[ShowInEditor, Serialize] private Actor[] spawnPoints;

	[ShowInEditor, Serialize] private float[] spawnTimes = [0f, 1.5f, 2f, 3f, 3.5f]; // Minutes Elapsed
	private int spawnTimeIndex = 0;
	private float currentSpawnTime = 0f;
	private const int MINUTES_TO_SECONDS = 60;
	private State state = State.WaitingForNextWave;

	private float elapsedTime = 0f;
	private float spawnTime = 0f;
	private int spawnIndex = 0;

	public override void OnAwake()
	{
		base.OnAwake();
		currentEnemiesToSpawn = (int)maxEnemies[enemiesToSpawnIndex];
		currentSpawnTime = spawnTimes[spawnTimeIndex] * MINUTES_TO_SECONDS;
	}


	public override void OnUpdate()
	{


		if (state == State.WaitingForNextWave)
		{
			elapsedTime += Time.DeltaTime;
			// Debug.Log($"MaxEnemis: {currentSpawnTime} <= {elapsedTime} = {currentSpawnTime <= elapsedTime}");
			if (spawnTimeIndex < spawnTimes.Length && currentSpawnTime <= elapsedTime)
			{
				state = State.SpawningEnemies;

				elapsedTime = 0f;
				// Debug.Log($"Max Enemies: {currentEnemiesToSpawn}");
			}
		}
		else if (state == State.SpawningEnemies)
		{

			spawnTime += Time.DeltaTime;
			if (spawnTime >= 1f)
			{
				spawnTime = 0f;


				var spawnPoint = spawnPoints[Random.Shared.Next(0, spawnPoints.Length)];
				var enemy = PrefabManager.SpawnPrefab(enemyPrefab, spawnPoint.Position, spawnPoint.Orientation);
				enemy.Name = "Enemy_" + spawnIndex;
				enemy.Parent = Actor;


				spawnIndex++;
				if (spawnIndex >= currentEnemiesToSpawn)
				{
					state = State.WaitingForNextWave;
					spawnIndex = 0;
					spawnTimeIndex++;
					enemiesToSpawnIndex++;
					currentEnemiesToSpawn += (int)spawnTimes[enemiesToSpawnIndex];
					currentSpawnTime = spawnTimes[spawnTimeIndex] * MINUTES_TO_SECONDS;
				}
			}
		}
	}

	private enum State
	{
		WaitingForNextWave,
		SpawningEnemies

	}

}
