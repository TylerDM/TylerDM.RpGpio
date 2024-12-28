namespace TylerDm.RpGpio.Devices.Pulsers;

public class Pulser : IDisposable, IDevice
{
	private readonly DisposedTracker<Pulser> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopWatch = new();
	private readonly IPinReader _pin;
	private readonly PinEventTypes _pulseStartEvent;
	private readonly PinEventTypes _pulseEndEvent;

	private event PinPulseStartedEvent? onPulseStart;
	private event PinPulseEndedEvent? onPulseEnd;

	public event PinPulseStartedEvent OnPulseStart
	{
		add
		{
			_disposed.ThrowIf();
			onPulseStart += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onPulseStart -= value;
		}
	}
	public event PinPulseEndedEvent OnPulseEnd
	{
		add
		{
			_disposed.ThrowIf();
			onPulseEnd += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onPulseEnd -= value;
		}
	}

	public PinNumber PinNumber => _pin.Number;

	public Pulser(IPinReader pin, bool restState = false)
	{
		_pin = pin;
		_pin.ValueChanged += handlePinChanged;

		//This allows the caller to specify what the expected rest state is.
		//From there we wait for the transition into the active (pulsed) state.
		//So if the rest state is true, then falling to false would be the pulse.
		_pulseStartEvent = restState ? PinEventTypes.Falling : PinEventTypes.Rising;
		_pulseEndEvent = restState ? PinEventTypes.Rising : PinEventTypes.Falling;
	}

	public async Task<bool> TryWaitForPulseAsync(Ct ct = default)
	{
		_disposed.ThrowIf();

		using var linkedCts = _disposed.CreateLinkedCts(ct);
		using var gate = new Gate();
		void handlePulse() => gate.Open();
		try
		{
			OnPulseStart += handlePulse;
			return await gate.TryWaitAsync(linkedCts.Token);
		}
		finally
		{
			OnPulseStart -= handlePulse;
		}
	}

	public void Dispose()
	{
		if (_disposed) return;
		_disposed.Dispose();

		_pin.ValueChanged -= handlePinChanged;
		_pin.Dispose();
	}

	private void handlePinChanged(PinEventTypes eventType)
	{
		lock (_lock)
		{
			if (eventType == _pulseStartEvent)
			{
				_stopWatch.Start();
				onPulseStart?.Invoke();
			}

			if (eventType == _pulseEndEvent)
			{
				_stopWatch.Stop();
				onPulseEnd?.Invoke(_stopWatch.Elapsed);
				_stopWatch.Reset();
			}
		}
	}
}
