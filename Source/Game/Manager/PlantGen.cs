using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PlantGen Script.
/// </summary>
public class PlantGen : Script
{
	[ShowInEditor, Serialize] private Actor[] normalPlantActors;
	[ShowInEditor, Serialize] private Actor[] specialPlantActors;

	[ShowInEditor, Serialize] private Prefab normalPlantPrefab;
	[ShowInEditor, Serialize] private Prefab specialPlantPrefab;

	public override void OnStart()
	{

		List<Actor> allNormPlantActors = new List<Actor>();
		foreach (var child in normalPlantActors)
		{
			allNormPlantActors.AddRange(child.Children);
		}

		foreach (var actor in allNormPlantActors)
		{
			var plantActor = PrefabManager.SpawnPrefab(normalPlantPrefab, actor.Position);
			plantActor.Parent = actor.Parent;
		}

		List<Actor> allSpecPlantActors = new List<Actor>();
		foreach (var child in specialPlantActors)
		{
			allSpecPlantActors.AddRange(child.Children);
		}

		foreach (var actor in allSpecPlantActors)
		{
			var plantActor = PrefabManager.SpawnPrefab(specialPlantPrefab, actor.Position);
			plantActor.Parent = actor.Parent;
		}

	}
}
