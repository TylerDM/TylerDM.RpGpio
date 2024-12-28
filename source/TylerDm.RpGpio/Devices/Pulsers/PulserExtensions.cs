namespace TylerDm.RpGpio.Devices.Pulsers;

public static class PulserExtensions
{
	public static Pulser OpenPulser(this IGpio gpio, PinNumber pin, PinReadModes mode = PinReadModes.InputPullDown) =>
		new(gpio.OpenRead(pin, mode));
}
