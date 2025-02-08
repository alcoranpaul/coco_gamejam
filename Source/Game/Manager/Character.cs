using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Character Script.
/// </summary>
public class Character : InstanceManagerScript, IDamage
{
	[ShowInEditor, Serialize]
	private MovementComponent.MovementArgs _movementArgs;
	[ShowInEditor, Serialize]
	private MovementComponent.CameraArgs _cameraArgs;
	[ShowInEditor, Serialize]
	private InteractionComponent.InteractionArgs _interactionArgs;
	[ShowInEditor, Serialize]
	private InventoryComponent.InventoryArgs _inventoryArgs;
	[ShowInEditor, Serialize]
	private ToxinComponent.ToxinArgs _toxinArgs;

	[ShowInEditor, Serialize] private float startingHealth = 100f;

	public HealthComponent HealthComponent { get; private set; }
	public ToxinComponent ToxinComponent { get; private set; }
	private MovementComponent _movementComponent;
	private InteractionComponent _interactionComponent;
	private InventoryComponent _inventoryComponent;
	private ThrowComponent _throwComponent;
	private bool isDead;

	public event EventHandler OnDeathEvent;

	[ShowInEditor, Serialize] private AudioClip[] deathClips;
	[ShowInEditor, Serialize] private AudioSource bgmAudioSource;


	public override void OnAwake()
	{
		base.OnAwake();
		_movementComponent = new MovementComponent(_movementArgs, _cameraArgs, Actor.As<CharacterController>(), _movementArgs.CharacterObj.As<AnimatedModel>());
		_interactionComponent = new InteractionComponent(_interactionArgs, Actor);
		_inventoryComponent = new InventoryComponent(_inventoryArgs, _movementArgs.CharacterObj);
		_throwComponent = new ThrowComponent();

		HealthComponent = new HealthComponent(startingHealth);
		HealthComponent.OnDeath += OnDeath;
		ToxinComponent = new ToxinComponent(_toxinArgs, healthComponent: HealthComponent);

		_inventoryComponent.OnToxinVialAdded += OnToxinVialAdded;
		_inventoryComponent.OnToxinVialRemoved += OnToxinVialRemoved;
		_throwComponent.OnThrowEnabled += OnThrowEnabled;
	}

	private void OnDeath(object sender, EventArgs e)
	{
		if (isDead) return;
		isDead = true;
		Screen.CursorLock = CursorLockMode.Clipped;
		Screen.CursorVisible = true;
		_movementArgs.AnimModel.GetParameter("isDead").Value = true;

		int randomIndex = Random.Shared.Next(0, deathClips.Length);
		AudioClip deathClip = deathClips[randomIndex];
		SingletonManager.Get<SFXManager>().PlayAudio(clip: deathClip, Actor.Position, 5f);

		OnDeathEvent?.Invoke(this, EventArgs.Empty);
	}

	public override void OnDisable()
	{
		_interactionComponent.OnDisable();
		_throwComponent.OnDisable();
		_inventoryComponent.OnDisable();
		_inventoryComponent.OnToxinVialAdded -= OnToxinVialAdded;
		_inventoryComponent.OnToxinVialRemoved -= OnToxinVialRemoved;
		_throwComponent.OnThrowEnabled -= OnThrowEnabled;
		base.OnDisable();
	}

	public override void OnStart()
	{
		Screen.CursorLock = CursorLockMode.Locked;
		Screen.CursorVisible = false;

		_movementComponent.Start();
		HealthComponent.Start();
		ToxinComponent.Start();
	}

	public override void OnUpdate()
	{
		if (isDead)
		{
			if (bgmAudioSource.Volume > 0f)
			{
				bgmAudioSource.Volume -= 0.005f;
			}
			return;

		}
		ToxinComponent.Update();
	}

	public override void OnFixedUpdate()
	{
		if (isDead) return;
		_movementComponent.Update();
	}

	private void OnThrowEnabled(object sender, EventArgs e)
	{
		Debug.Log($"Throw has been raised");
	}

	private void OnToxinVialAdded(bool flag, DVial.Toxin toxin)
	{
		ToxinComponent.Toxify(flag, toxin);
	}


	private void OnToxinVialRemoved(DVial.Toxin toxin)
	{
		ToxinComponent.Toxify(false, toxin);
	}

	public void TakeDamage(float damage)
	{
		HealthComponent.Damage(damage);
	}

}


public interface IDamage
{
	void TakeDamage(float damage);
}