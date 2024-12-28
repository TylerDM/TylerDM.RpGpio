namespace TylerDm.RpGpio.Devices.Relays;

public static class RelayExtensions
{
	public static Relay OpenRelay(this IGpio gpio, PinNumber pin) =>
		new(gpio.OpenWrite(pin));
}
