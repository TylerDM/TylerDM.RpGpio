namespace TylerDm.RpGpio.Devices.Leds;

public static class LedOverloads
{
	public static async Task FlashUntilAsync(this Led led, TimeSpan duration, FlashSettings? settings = default)
	{
		using var durationCts = new Cts(duration);
		await led.FlashUntilAsync(durationCts.Token, settings);
	}
}
