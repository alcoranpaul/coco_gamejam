using System;
using FlaxEngine;

namespace Game;

/// <summary>
/// ToxinComponent class.
/// </summary>
public class ToxinComponent
{
	public float Toxin { get; private set; }
	public float MaxToxin { get; private set; }
	public float Normalized => Toxin / MaxToxin;

	public event Action<float, float, float> OnToxinChanged;
	public event EventHandler OnFull;

	private bool _toxified = false;
	private HealthComponent _healthComponent;
	private ToxinArgs _toxinArgs;



	public ToxinComponent(ToxinArgs _toxinArgs, HealthComponent healthComponent)
	{
		this._toxinArgs = _toxinArgs;
		MaxToxin = _toxinArgs.MaxToxin;
		Toxin = 0; // Start at 0 instead of max
		_healthComponent = healthComponent;
	}

	public void Start()
	{
		OnToxinChanged?.Invoke(Toxin, MaxToxin, Normalized);
	}

	public void Toxify(bool flag)
	{
		_toxified = flag;
	}

	public void Update()
	{
		if (_toxified)
		{
			Increase(_toxinArgs.FillRate * Time.DeltaTime);


		}
		else
		{
			Decrease(_toxinArgs.DecayRate * Time.DeltaTime);
		}

		if (Toxin > 50f)
		{
			float toxinFactor = (Toxin - 50f) / 50f; // Scales from 0 to 1 (50%-100%)
			float damagePerSecond = Mathf.Lerp(_healthComponent.MaxHealth * 0.01f, _healthComponent.MaxHealth * 0.03f, toxinFactor);

			_healthComponent.Damage(damagePerSecond * Time.DeltaTime);
		}
	}

	private void Decrease(float amount)
	{

		Toxin = Mathf.Max(Toxin - amount, 0);

		OnToxinChanged?.Invoke(Toxin, MaxToxin, Normalized);
	}

	private void Increase(float amount)
	{
		if (Toxin >= MaxToxin) return;
		Toxin = Mathf.Min(Toxin + amount, MaxToxin);

		if (Toxin >= MaxToxin)
		{
			OnFull?.Invoke(this, EventArgs.Empty);
		}
		OnToxinChanged?.Invoke(Toxin, MaxToxin, Normalized);
	}

	public class ToxinArgs
	{
		public float FillRate = 1f; // % per second
		public float DecayRate = 1f; // % per second when not toxified
		public float MaxToxin = 100f; // % per second when not toxified

	}
}
