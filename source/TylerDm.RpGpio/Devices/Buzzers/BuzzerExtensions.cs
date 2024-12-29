namespace TylerDm.RpGpio.Devices.Buzzers;

public static class BuzzerExtensions
{
	public static Buzzer OpenBuzzer(this IGpio gpio, PinNumber pin) =>
		new(gpio.OpenWrite(pin));

	public static async Task BuzzAsync(this Buzzer buzzer, TimeSpan duration = default)
	{
		if (duration == default) duration = TimeSpan.FromSeconds(1);

		buzzer.On = true;
		try
		{
			await Task.Delay(duration);
		}
		finally
		{
			buzzer.On = false;
		}
	}

	public static async Task BuzzAsync(this Buzzer buzzer, CancellationToken ct)
	{
		buzzer.On = true;
		try
		{
			await ct.WaitForCancelAsync();
		}
		finally
		{
			buzzer.On = false;
		}
	}
}
