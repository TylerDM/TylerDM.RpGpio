namespace TylerDm.RpGpio.Devices.Buttons;

public class Button(IPinReader pin) : ReadingDevice(pin)
{
	private readonly DisposedTracker<Button> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopwatch = new();

	private event ButtonPressed? onPressedEvent;
	private event ButtonReleased? onReleasedEvent;

	public event ButtonPressed OnPressedEvent
	{
		add
		{
			_disposed.ThrowIf();
			onPressedEvent += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onPressedEvent -= value;
		}
	}
	public event ButtonReleased OnReleasedEvent
	{
		add
		{
			_disposed.ThrowIf();
			onReleasedEvent += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onReleasedEvent -= value;
		}
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		onPressedEvent = null;
		onReleasedEvent = null;
		base.Dispose();
	}

	protected override void handleValueChanged(PinEventTypes type)
	{
		lock (_lock)
		{
			if (type == PinEventTypes.Rising)
			{
				_stopwatch.Start();
				onPressedEvent?.Invoke();
			}

			if (type == PinEventTypes.Falling)
			{
				_stopwatch.Stop();
				onReleasedEvent?.Invoke(_stopwatch.Elapsed);
				_stopwatch.Reset();
			}
		}
	}
}
