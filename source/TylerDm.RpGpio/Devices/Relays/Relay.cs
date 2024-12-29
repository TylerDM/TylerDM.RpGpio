namespace TylerDm.RpGpio.Devices.Relays;

public class Relay(IPinWriter pin) : WritingDevice(pin)
{
	public bool Energized
	{
		get => getValue();
		set => setValue(value);
	}
}
