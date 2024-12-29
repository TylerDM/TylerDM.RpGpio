namespace TylerDm.RpGpio.Devices.Relays;

public class Relay : WritingDevice
{
	private readonly DisposedTracker<Relay> _disposed = new();

	public bool Energized
	{
		get => getValue();
		set => setValue(value);
	}

	public Relay(IPinWriter pin) : base(pin)
	{
		setValue(false);
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		base.Dispose();
	}
}
