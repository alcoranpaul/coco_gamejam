using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// HealthUI Script.
/// </summary>
public class ToxinUI : Script
{
    [ShowInEditor, Serialize] private UIControl toxinLabelControl;
    [ShowInEditor, Serialize] private UIControl progressBarControl;
    [ShowInEditor, Serialize] private Character character;


    private Label toxinLabel;
    private ProgressBar progressBar;

    public override void OnStart()
    {
        toxinLabel = toxinLabelControl.Get<Label>();
        progressBar = progressBarControl.Get<ProgressBar>();

        character.ToxinComponent.OnToxinChanged += OnToxinChanged;
    }

    public override void OnDisable()
    {
        character.ToxinComponent.OnToxinChanged -= OnToxinChanged;
        base.OnDisable();
    }

    private void OnToxinChanged(float toxin, float maxToxin, float normalized)
    {
        toxinLabel.Text = $"{toxin:F1}%";
        progressBar.Value = normalized;


    }

}
