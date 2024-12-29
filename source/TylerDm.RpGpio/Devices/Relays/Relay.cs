namespace TylerDm.RpGpio.Devices.Relays;

public class Relay(IPinWriter pin) : WritingDevice(pin)
{
	private readonly DisposedTracker<Relay> _disposed = new();

	public bool Energized
	{
		get => getValue();
		set => setValue(value);
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		base.Dispose();
	}
}
