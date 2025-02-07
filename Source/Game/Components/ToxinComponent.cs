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


	private HealthComponent _healthComponent;
	private ToxinArgs _toxinArgs;

	private bool _isHoldingNormalVial = false;
	private bool _isHoldingSpecialVial = false;

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

	public void Toxify(bool flag, DVial.Toxin toxin)
	{
		if (toxin == DVial.Toxin.Normal)
			_isHoldingNormalVial = flag;

		else
			_isHoldingSpecialVial = flag;

	}

	public void Update()
	{
		if (_isHoldingNormalVial || _isHoldingSpecialVial)
		{
			if (_isHoldingSpecialVial)
				Increase(_toxinArgs.SpecialFillRate * Time.DeltaTime);
			if (_isHoldingNormalVial)
				Increase(_toxinArgs.NormalFillRate * Time.DeltaTime);
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
		public float NormalFillRate = 1f; // % per second
		public float SpecialFillRate = 0.5f; // % per second
		public float DecayRate = 1.3f; // % per second when not toxified
		public float MaxToxin = 100f; // % per second when not toxified

	}
}
