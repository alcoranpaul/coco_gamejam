using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// FoliageGen Script.
/// </summary>
public class FoliageGen : Script
{
	public Foliage foliage;
	public CollisionData type1Col;
	public CollisionData type2Col;
	public CollisionData type3Col;
	public CollisionData type4Col;


	/// <inheritdoc/>
	public override void OnStart()
	{
		for (int i = 0; i < foliage.InstancesCount; i++)
		{
			AddCollider(foliage.GetInstance(i).Type, i);
		}
	}

	private void AddCollider(int type, int index)
	{
		MeshCollider meshCollider = new MeshCollider();
		meshCollider.Parent = Actor;
		switch (type)
		{
			case 0:
				meshCollider.CollisionData = type1Col;
				break;
			case 1:
				meshCollider.CollisionData = type2Col;
				break;
			case 2:
				meshCollider.CollisionData = type3Col;
				break;
			case 3:
				meshCollider.CollisionData = type4Col;
				break;
		}

		meshCollider.Position = foliage.GetInstance(index).Transform.Translation;
		meshCollider.Scale = foliage.GetInstance(index).Transform.Scale;
		meshCollider.Rotation = foliage.GetInstance(index).Transform.GetRotation();
	}


}
