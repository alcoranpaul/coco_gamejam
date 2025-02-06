using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// BTD_WithinRange Script.
/// </summary>
public class BTD_WithinRange : BehaviorTreeDecorator
{
	public float Range = 100.0f;
	public EComparison Comparison = EComparison.LessOrEqual;
	// public BehaviorKnowledgeSelector<Actor> PlayerSelector;

	public override bool CanUpdate(BehaviorUpdateContext context)
	{
		// Debug.Log($"PlayerSelector: {PlayerSelector == null}");
		// if (!PlayerSelector.TryGet(context.Knowledge, out var playerActor))
		// 	return false;

		Actor playerActor = SingletonManager.Get<Character>().Actor;
		if (playerActor == null)
			return false;
		Vector3 agentPos = context.Behavior.Actor.Position;
		Vector3 playerPos = playerActor.Position;

		float distance = Vector3.Distance(agentPos, playerPos);

		bool retVal = Comparison switch
		{
			EComparison.Equal => distance == Range,
			EComparison.NotEqual => distance != Range,
			EComparison.Less => distance < Range,
			EComparison.LessOrEqual => distance <= Range,
			EComparison.Greater => distance > Range,
			EComparison.GreaterOrEqual => distance >= Range,
			_ => false,
		};

		Debug.Log($"Distance: {distance}, Range: {Range}, Comparison: {Comparison}, retVal: {retVal}");
		// Here you can interact with level or gameplay objects (be aware that this code runs in async by default)
		return retVal;
	}

	public enum EComparison
	{
		Equal,
		NotEqual,
		Less,
		LessOrEqual,
		Greater,
		GreaterOrEqual,
	}

}
