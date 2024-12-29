﻿namespace TylerDm.RpGpio.Devices.Buttons;

public class Button(IPinReader pin) : ReadingDevice(pin)
{
	private readonly DisposedTracker<Button> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopwatch = new();

	private event ButtonPressed? onButtonPressed;
	private event ButtonReleased? onButtonReleased;

	public event ButtonPressed OnButtonPressed
	{
		add
		{
			_disposed.ThrowIf();
			onButtonPressed += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onButtonPressed -= value;
		}
	}
	public event ButtonReleased OnButtonReleased
	{
		add
		{
			_disposed.ThrowIf();
			onButtonReleased += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onButtonReleased -= value;
		}
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		onButtonPressed = null;
		onButtonReleased = null;
		base.Dispose();
	}

	protected override void handleValueChanged(PinEventTypes type)
	{
		lock (_lock)
		{
			if (type == PinEventTypes.Rising)
			{
				_stopwatch.Start();
				onButtonPressed?.Invoke();
			}

			if (type == PinEventTypes.Falling)
			{
				_stopwatch.Stop();
				onButtonReleased?.Invoke(_stopwatch.Elapsed);
				_stopwatch.Reset();
			}
		}
	}
}
