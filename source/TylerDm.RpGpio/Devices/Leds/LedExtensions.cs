namespace TylerDm.RpGpio.Devices.Leds;

public static class LedExtensions
{
	public static Led OpenLed(this IGpio gpio, PinNumber pin) =>
		new(gpio.OpenWrite(pin));

	public static void Toggle(this Led led) =>
		led.On = !led.On;

	public static async Task FlashAsync(this Led led, CancellationToken ct)
	{
		led.On = true;
		await ct.WaitForCancelAsync();
		led.On = false;
	}

	public static async Task FlashAsync(this Led led, TimeSpan duration = default)
	{
		if (duration == default) duration = TimeSpan.FromSeconds(1);

		led.On = true;
		await Task.Delay(duration);
		led.On = false;
	}
}
