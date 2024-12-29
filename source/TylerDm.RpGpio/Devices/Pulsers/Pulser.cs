namespace TylerDm.RpGpio.Devices.Pulsers;

public class Pulser : ReadingDevice
{
	private readonly DisposedTracker<Pulser> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopWatch = new();
	private readonly PinEventTypes _pulseStartEvent;
	private readonly PinEventTypes _pulseEndEvent;

	private event PulseStarted? onPulseStart;
	private event PulseEnded? onPulseEnd;

	public event PulseStarted OnPulseStart
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
	public event PulseEnded OnPulseEnd
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

	public Pulser(IPinReader pin, bool restState = false) : base(pin)
	{
		//This allows the caller to specify what the expected rest state is.
		//From there we wait for the transition into the active (pulsed) state.
		//So if the rest state is true, then falling to false would be the pulse.
		_pulseStartEvent = restState ? PinEventTypes.Falling : PinEventTypes.Rising;
		_pulseEndEvent = restState ? PinEventTypes.Rising : PinEventTypes.Falling;
	}

	public override void Dispose()
	{
		if (_disposed) return;
		_disposed.Dispose();

		base.Dispose();
	}

	protected override void handleValueChanged(PinEventTypes eventType)
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
