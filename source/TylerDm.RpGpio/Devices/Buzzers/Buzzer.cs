namespace TylerDm.RpGpio.Devices.Buzzers;

public class Buzzer : WritingDevice
{
	private readonly DisposedTracker<Buzzer> _disposed = new();

	public bool On
	{
		get => getValue();
		set => setValue(value);
	}

	public Buzzer(IPinWriter pin, bool? initialState = false) : base(pin)
	{
		if (initialState is bool b)
			pin.Write(b);
	}

	public override void Dispose()
	{
		if (_disposed.Dispose()) return;

		On = false;
		base.Dispose();
	}
}
