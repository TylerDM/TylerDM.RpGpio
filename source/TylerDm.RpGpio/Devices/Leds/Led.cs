namespace TylerDm.RpGpio.Devices.Leds;

public class Led : WritingDevice
{
	private readonly DisposedTracker<Led> _disposed = new();

	public bool On
	{
		get => getValue();
		set => setValue(value);
	}

	public Led(IPinWriter pin) : base(pin)
	{
		On = false;
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		On = false;
		base.Dispose();
	}
}
