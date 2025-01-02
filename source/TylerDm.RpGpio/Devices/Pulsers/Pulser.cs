namespace TylerDm.RpGpio.Devices.Pulsers;

public class Pulser(IPinReader pin) : ReadingDevice(pin)
{
	private readonly DisposedTracker<Pulser> _disposed = new();
	private readonly Lock _lock = new();
	private readonly Stopwatch _stopWatch = new();

	private event PulseStarted? onPulseStarted;
	private event PulseEnded? onPulseEnded;

	public event PulseStarted OnPulseStarted
	{
		add
		{
			_disposed.ThrowIf();
			onPulseStarted += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onPulseStarted -= value;
		}
	}
	public event PulseEnded OnPulseEnded
	{
		add
		{
			_disposed.ThrowIf();
			onPulseEnded += value;
		}
		remove
		{
			_disposed.ThrowIf();
			onPulseEnded -= value;
		}
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		base.Dispose();
	}

	protected override void handleValueChanged(PinEventTypes eventType)
	{
		lock (_lock)
		{
			if (eventType == _activatingEvent)
			{
				_stopWatch.Start();
				onPulseStarted?.Invoke();
			}

			if (eventType == _deactivatingEvent)
			{
				_stopWatch.Stop();
				onPulseEnded?.Invoke(_stopWatch.Elapsed);
				_stopWatch.Reset();
			}
		}
	}
}
