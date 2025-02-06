using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// InputManager Script.
/// </summary>
public class InputManager : InstanceManagerScript
{

	public InputAxis MouseXAxis { get; private set; }
	public InputAxis MouseYAxis { get; private set; }
	public InputAxis MoveHAxis { get; private set; }
	public InputAxis MoveVAxis { get; private set; }

	public event Action OnHideItem;
	public event Action OnSwapVial;
	public event Action OnUseVial;
	public event Action OnCollectVial;

	private InputEvent hideItemEvent;
	private InputEvent swapVialEvent;
	private InputEvent useVialEvent;
	private InputEvent collectVialEvent;

	public override void OnAwake()
	{
		base.OnAwake();

		MouseXAxis = new InputAxis("Mouse X");
		MouseYAxis = new InputAxis("Mouse Y");

		MoveHAxis = new InputAxis("Horizontal");
		MoveVAxis = new InputAxis("Vertical");

		hideItemEvent = new InputEvent("HideItem");
		hideItemEvent.Pressed += () => { OnHideItem?.Invoke(); };

		swapVialEvent = new InputEvent("SwapVial");
		swapVialEvent.Pressed += () => { OnSwapVial?.Invoke(); };

		useVialEvent = new InputEvent("UseVial");
		useVialEvent.Pressed += () => { OnUseVial?.Invoke(); };

		collectVialEvent = new InputEvent("CollectVial");
		collectVialEvent.Pressed += () => { OnCollectVial?.Invoke(); };
	}

	public override void OnDisable()
	{
		MoveHAxis.Dispose();
		MoveVAxis.Dispose();
		MouseXAxis.Dispose();
		MouseYAxis.Dispose();

		hideItemEvent.Dispose();
		swapVialEvent.Dispose();
		useVialEvent.Dispose();
		collectVialEvent.Dispose();

		OnHideItem = null;
		OnSwapVial = null;
		OnUseVial = null;
		OnCollectVial = null;

		base.OnDisable();
	}


}
