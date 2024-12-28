namespace TylerDm.RpGpio.Devices.Buzzers;

public static class BuzzerExtensions
{
	public static Buzzer OpenBuzzer(this IGpio gpio, PinNumber pin) =>
		new(gpio.OpenWrite(pin));
}
