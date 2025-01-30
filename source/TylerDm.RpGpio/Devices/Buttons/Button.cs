namespace TylerDm.RpGpio.Devices.Buttons;

public class Button(IPinReader pin) : ReadingDevice(pin)
{
	private readonly DisposedTracker<Button> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopwatch = new();

	private bool pressed = false;

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

	public TimeSpan MinimumDuration { get; set; } = TimeSpan.Zero;

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
			if (type == _activatingEvent)
			{
				//Prevent double rising events.
				if (pressed) return;
				pressed = true;

				_stopwatch.Start();
				onButtonPressed?.Invoke();
			}

			if (type == _deactivatingEvent)
			{
				//Prevent double rising events.
				pressed = false;

				var elapsed = _stopwatch.Elapsed;
				if (elapsed > MinimumDuration)
					onButtonReleased?.Invoke(elapsed);
				_stopwatch.Reset();
			}
		}
	}
}
