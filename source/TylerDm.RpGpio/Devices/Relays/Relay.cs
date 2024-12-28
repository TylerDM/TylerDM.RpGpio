namespace TylerDm.RpGpio.Devices.Relays;

public class Relay : WritingDevice
{
	public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(100);

	private readonly DisposedTracker<Relay> _disposed = new();
	private readonly bool _energizedState;

	private Cts cts = new();

	public bool Energized
	{
		get => getValue() != _energizedState;
		set
		{
			methodPreflight();

			setValue(value);
		}
	}

	public Relay(IPinWriter pin, bool energizedState = true) : base(pin)
	{
		_energizedState = energizedState;
		setValue(_energizedState);
	}

	public async Task PulseAsync(TimeSpan duration, Ct ct)
	{
		methodPreflight();

		if (duration == default) duration = DefaultDuration;

		using var linkedCts = cts.CreateLinked(ct);
		try
		{
			setValue(true);
			await duration.WaitAsync(linkedCts);
		}
		finally
		{
			setValue(false);
		}
	}

	public async Task PulseAsync(TimeSpan duration = default)
	{
		methodPreflight();

		if (duration == default) duration = DefaultDuration;

		try
		{
			setValue(true);
			await duration.WaitAsync(cts);
		}
		finally
		{
			setValue(false);
		}
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		base.Dispose();
	}

	private void methodPreflight()
	{
		_disposed.ThrowIf();
		cancelPreviousOperations();
	}

	private void cancelPreviousOperations()
	{
		cts.Dispose();
		cts = new();
	}
}
