namespace TylerDm.RpGpio.Devices.Buttons;

public class Button(IPinReader pin) : ReadingDevice(pin)
{
	internal readonly DisposedTracker<Button> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopwatch = new();

	private event ButtonPressedEvent? onPressedEvent;
	private event ButtonReleasedEvent? onReleasedEvent;

	public event ButtonPressedEvent OnPressedEvent
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
	public event ButtonReleasedEvent OnReleasedEvent
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

	public async Task<bool> TryWaitForPressAsync(Ct ct)
	{
		_disposed.ThrowIf();

		using var linkedCts = _disposed.CreateLinkedCts(ct);
		var token = linkedCts.Token;

		using var gate = new Gate();
		void handlePressed() => gate.Open();

		OnPressedEvent += handlePressed;
		try
		{
			return await gate.TryWaitAsync(token);
		}
		finally
		{
			OnPressedEvent -= handlePressed;
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
