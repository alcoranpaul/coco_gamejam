using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// InputManager Script.
/// </summary>
public class InputManager : InstanceManagerScript
{
	[ShowInEditor, Serialize] private UIControl pauseControl;
	[ShowInEditor, Serialize] private UIControl resumeControl;

	private Button continueButton;


	public InputAxis MouseXAxis { get; private set; }
	public InputAxis MouseYAxis { get; private set; }
	public InputAxis MoveHAxis { get; private set; }
	public InputAxis MoveVAxis { get; private set; }

	public event Action OnHideItem;
	public event Action OnSwapVial;
	public event Action OnUseVial;
	public event Action OnCollectVial;
	public event Action OnMouseRelease;
	public event Action OnPaused;

	private InputEvent hideItemEvent;
	private InputEvent swapVialEvent;
	private InputEvent useVialEvent;
	private InputEvent collectVialEvent;
	private InputEvent mouseReleaseEvent;
	private InputEvent pauseEvent;

	public event EventHandler OnResume;

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

		mouseReleaseEvent = new InputEvent("MouseRelease");
		mouseReleaseEvent.Released += () => { OnMouseRelease?.Invoke(); };

		pauseEvent = new InputEvent("Pause");
		pauseEvent.Pressed += Pause;

		continueButton = resumeControl.Get<Button>();
		continueButton.Clicked += Resume;

		pauseControl.IsActive = false;

	}

	private void Resume()
	{
		Screen.CursorLock = CursorLockMode.Locked;
		Screen.CursorVisible = false;
		pauseControl.IsActive = false;
		Time.TimeScale = 1f;
		OnResume?.Invoke(this, EventArgs.Empty);

	}

	private void Pause()
	{
		pauseControl.IsActive = true;
		Screen.CursorLock = CursorLockMode.Clipped;
		Screen.CursorVisible = true;
		Time.TimeScale = 0f;
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
		mouseReleaseEvent.Dispose();
		pauseEvent.Dispose();

		OnHideItem = null;
		OnSwapVial = null;
		OnUseVial = null;
		OnCollectVial = null;
		OnMouseRelease = null;
		OnPaused = null;

		continueButton.Clicked -= Resume;

		base.OnDisable();
	}


}
