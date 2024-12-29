namespace TylerDm.RpGpio.Devices.Leds;

public class Led : WritingDevice
{
	public bool On
	{
		get => getValue();
		set => setValue(value);
	}

	public Led(IPinWriter pin, bool? defaultState = false) : base(pin)
	{
		if (defaultState is bool ds)
			setValue(ds);
	}
}
