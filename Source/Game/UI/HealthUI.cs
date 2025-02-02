using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// HealthUI Script.
/// </summary>
public class HealthUI : Script
{
	[ShowInEditor, Serialize] private UIControl healthLabelControl;
	[ShowInEditor, Serialize] private UIControl progressBarControl;
	[ShowInEditor, Serialize] private Character character;


	private Label healthLabel;
	private ProgressBar progressBar;

	public override void OnStart()
	{
		healthLabel = healthLabelControl.Get<Label>();
		progressBar = progressBarControl.Get<ProgressBar>();

		character.HealthComponent.OnHealthChanged += OnHealthChanged;
	}

	public override void OnDisable()
	{
		character.HealthComponent.OnHealthChanged -= OnHealthChanged;
		base.OnDisable();
	}

	private void OnHealthChanged(float health, float maxHealth, float normalizedHealth)
	{
		healthLabel.Text = $"{health}/{maxHealth}";
		progressBar.Value = normalizedHealth;
		Debug.Log($"Health changed: {health}/{maxHealth} ({normalizedHealth:P})");

	}

}
