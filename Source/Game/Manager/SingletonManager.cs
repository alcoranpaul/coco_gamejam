using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace Game;

/// <summary>
/// SingletonManager Script.
/// </summary>

public class SingletonManager : GamePlugin
{
	private Dictionary<Type, object> singletons;


	/// <inheritdoc />
	public SingletonManager()
	{
		_description = new PluginDescription
		{
			Name = "Singleton Manager",
			Category = "System",
			Author = "alcoranpaul",
			AuthorUrl = "https://github.com/alcoranpaul",
			HomepageUrl = "https://github.com/alcoranpaul",
			RepositoryUrl = "",
			Description = "A manager for singletons",
			Version = new Version(1, 0),
			IsAlpha = false,
			IsBeta = false,
		};
	}

	/// <inheritdoc />
	public override void Initialize()
	{
		base.Initialize();

		singletons = new Dictionary<Type, object>();

	}

	/// <inheritdoc />
	public override void Deinitialize()
	{
		// Use it to cleanup data
		ClearAll();
		base.Deinitialize();
	}

	// Register a singleton instance
	public void Register<T>(T instance) where T : class
	{
		Type type = instance.GetType();

		if (singletons.TryGetValue(type, out var value))
		{
			Debug.LogWarning($"Singleton of type {type} is already registered.\nDestroying the old instance.");

			if (value is Script)
				Destroy(value as Script);
			else
				singletons[type] = null;
		}
		singletons[type] = instance;
		Debug.Log($"Singleton of type {type} is registered.");
	}

	// Unregister a singleton instance
	public void Unregister<T>() where T : class
	{
		var type = typeof(T);
		if (singletons.ContainsKey(type))
		{
			singletons[type] = null;
		}
		Debug.Log($"Singleton of type {type} is unregistered.");
	}

	// Get the singleton instance
	private T GetInstance<T>() where T : class
	{
		if (singletons == null || !singletons.TryGetValue(typeof(T), out object instance))
			return null;

		return instance as T;
	}

	public static T Get<T>() where T : class
	{
		return PluginManager.GetPlugin<SingletonManager>()?.GetInstance<T>();
	}

	// Nullify all singletons (for example, when exiting play mode)
	public void ClearAll()
	{
		foreach (Type key in singletons.Keys.ToList())
		{
			if (singletons[key] is InstanceManagerScript)
				Destroy(singletons[key] as InstanceManagerScript);
			else if (singletons[key] is InstanceManagerClass)
				(singletons[key] as InstanceManagerClass).Destroy();
			singletons[key] = null;
		}
		Debug.Log("All singletons are cleared.");
	}


}

/// <summary>
/// Purpose of this class is to register the instance of the class to the SingletonManager
/// </summary>
public abstract class InstanceManagerScript : Script
{
	/// <inheritdoc/>
	public override void OnAwake()
	{
		if (GetType() != typeof(InstanceManagerScript) && GetType().BaseType == typeof(InstanceManagerScript))
		{
			PluginManager.GetPlugin<SingletonManager>().Register(this);
		}
	}

}

/// <summary>
/// Purpose of this class is to register the instance of the class to the SingletonManager
/// </summary>
public abstract class InstanceManagerClass
{
	public InstanceManagerClass()
	{
		if (GetType() != typeof(InstanceManagerClass) && GetType().BaseType == typeof(InstanceManagerClass))
		{
			PluginManager.GetPlugin<SingletonManager>().Register(this);
		}
	}

	public virtual void Destroy()
	{

	}
}