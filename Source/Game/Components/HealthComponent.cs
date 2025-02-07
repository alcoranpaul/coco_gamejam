using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// HealthComponent class.
/// </summary>
public class HealthComponent
{
	public float Health { get; private set; }
	public float MaxHealth { get; private set; }
	public float NormalizedHealth => Health / MaxHealth;

	public event Action<float, float, float> OnHealthChanged;
	public event EventHandler OnDeath;

	public HealthComponent(float maxHealth)
	{
		this.MaxHealth = maxHealth;
		Health = maxHealth;
	}

	public void Start()
	{
		OnHealthChanged?.Invoke(Health, MaxHealth, NormalizedHealth);
	}

	public void Damage(float amount)
	{
		Health -= amount;
		if (Health < 0)
		{
			Health = 0;
			// LOSE CONDITIOn
			OnDeath?.Invoke(this, EventArgs.Empty);
		}
		OnHealthChanged?.Invoke(Health, MaxHealth, NormalizedHealth);
	}

	public void Heal(float amount)
	{
		Health += amount;
		if (Health > MaxHealth)
			Health = MaxHealth;
		OnHealthChanged?.Invoke(Health, MaxHealth, NormalizedHealth);
	}


}
