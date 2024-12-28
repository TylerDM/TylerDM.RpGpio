namespace TylerDm.RpGpio.Devices.Leds;

public static class LedExtensions
{
	public static Led OpenLed(this IGpio gpio, PinNumber pin, bool? defaultState = false) =>
		new(gpio.OpenWrite(pin), defaultState);
}
