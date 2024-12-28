namespace TylerDm.RpGpio.Devices.Buttons;

public class Button : IDisposable, IDevice
{
	internal readonly DisposedTracker<Button> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopwatch = new();

	private readonly IPinReader _pin;

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

	public PinNumber PinNumber => _pin.Number;

	public Button(IPinReader pin)
	{
		_pin = pin;
		_pin.ValueChanged += handlePinChanged;
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

	public void Dispose()
	{
		if (_disposed.Dispose()) return;

		onPressedEvent = null;
		onReleasedEvent = null;
		_pin.ValueChanged -= handlePinChanged;
		_pin.Dispose();
	}

	private void handlePinChanged(PinEventTypes type)
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
