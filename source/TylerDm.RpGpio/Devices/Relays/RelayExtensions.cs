namespace TylerDm.RpGpio.Devices.Relays;

public static class RelayExtensions
{
	private static readonly TimeSpan _defaultDuration = TimeSpan.FromMilliseconds(100);

	public static Relay OpenRelay(this IGpio gpio, PinNumber pin) =>
		new(gpio.OpenWrite(pin));

	public static BgTaskHandle ToggleScope(this Relay relay) =>
		new(relay.ToggleAsync);

	public static async Task ToggleAsync(this Relay relay, CancellationToken ct)
	{
		try
		{
			relay.Energized = true;
			await ct.WaitForCancelAsync();
		}
		finally
		{
			relay.Energized = false;
		}
	}

	public static async Task PulseAsync(this Relay relay, int count, TimeSpan onDuration = default, TimeSpan offDuration = default, CancellationToken ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		if (onDuration == default) onDuration = _defaultDuration;
		if (offDuration == default) offDuration = _defaultDuration;

		for (int i = 0; i < count; i++)
		{
			ct.ThrowIfCancellationRequested();

			await relay.PulseAsync(onDuration, ct);
			await offDuration.WaitAsync(ct);
		}
	}

	public static async Task PulseAsync(this Relay relay, TimeSpan duration = default, CancellationToken ct = default)
	{
		if (duration == default) duration = _defaultDuration;

		try
		{
			relay.Energized = true;
			await duration.WaitAsync(ct);
		}
		finally
		{
			relay.Energized = false;
		}
	}
}
