namespace TylerDm.RpGpio.Devices.Buzzers;

public class Buzzer : WritingDevice
{
	public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(1);

	//CTS from _disposed not to be used here as we need a resettable one.
	private readonly DisposedTracker<Buzzer> _disposed = new();

	private Cts cts = new();

	public bool Buzzing
	{
		get => getValue();
		set => setValue(value);
	}

	public Buzzer(IPinWriter pin) : base(pin)
	{
		pin.Write(false);
	}

	public async Task BuzzAsync(int count, TimeSpan onDuration = default, TimeSpan offDuration = default, Ct ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		methodPreflight();

		if (onDuration == default) onDuration = DefaultDuration;
		if (offDuration == default) offDuration = DefaultDuration;

		using var linkedCts = cts.CreateLinked(ct);
		ct = linkedCts.Token;

		for (int i = 0; i < count; i++)
		{
			if (ct.IsCancellationRequested) break;

			setValue(true);
			await onDuration.WaitAsync(ct);

			setValue(false);
			await offDuration.WaitAsync(ct);
		}
	}

	public async Task BuzzAsync(Ct ct)
	{
		methodPreflight();

		using var linkedCts = _disposed.CreateLinkedCts(ct);

		try
		{
			setValue(true);
			await linkedCts.Token.WaitForCancelAsync();
		}
		finally
		{
			setValue(false);
		}
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		cts.Dispose();
		base.Dispose();
	}

	private void methodPreflight()
	{
		_disposed.ThrowIf();

		cancelExistingOperations();
	}

	private void cancelExistingOperations()
	{
		cts.Dispose();
		cts = new();
	}
}
