using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// HealthComponent class.
/// </summary>
public class HealthComponent
{
	private float health;
	private float maxHealth;
	public float NormalizedHealth => health / maxHealth;

	public event Action<float> OnHealthChanged;
	public event EventHandler OnDeath;

	public HealthComponent(float maxHealth)
	{
		this.maxHealth = maxHealth;
		health = maxHealth;
	}

	public void Damage(float amount)
	{
		health -= amount;
		if (health < 0)
		{
			health = 0;
			OnDeath?.Invoke(this, EventArgs.Empty);
		}
		OnHealthChanged?.Invoke(health);
	}

	public void Heal(float amount)
	{
		health += amount;
		if (health > maxHealth)
			health = maxHealth;
		OnHealthChanged?.Invoke(health);
	}



}
