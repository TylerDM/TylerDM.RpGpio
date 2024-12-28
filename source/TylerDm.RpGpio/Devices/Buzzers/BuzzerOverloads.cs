namespace TylerDm.RpGpio.Devices.Buzzers;

public static class BuzzerOverloads
{
	public static Task BuzzAsync(this Buzzer buzzer, int count, TimeSpan duration) =>
		buzzer.BuzzAsync(count, duration, duration);

	public static async Task BuzzAsync(this Buzzer buzzer, TimeSpan duration = default)
	{
		if (duration == default) duration = Buzzer.DefaultDuration;

		using var timedCts = new Cts(duration);
		await buzzer.BuzzAsync(timedCts.Token);
	}
}
