namespace TylerDm.RpGpio.Devices.Relays;

public class Relay : WritingDevice
{
	private readonly DisposedTracker<Relay> _disposed = new();
	private readonly bool _energizedState;

	public bool Energized
	{
		get => getValue();
		set => setValue(value);
	}

	public Relay(IPinWriter pin, bool energizedState = true) : base(pin)
	{
		_energizedState = energizedState;
		setValue(_energizedState);
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		base.Dispose();
	}
}
