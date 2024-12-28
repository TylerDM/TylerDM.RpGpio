namespace TylerDm.RpGpio.Devices.Relays;

public static class RelayOverloads
{
	public static async Task PulseAsync(this Relay relay, int count, TimeSpan onDuration = default, TimeSpan offduration = default, Ct ct = default)
	{
		if (offduration == default) offduration = Relay.DefaultDuration;

		for (int i = 0; i < count; i++)
		{
			if (ct.IsCancellationRequested) break;

			if (ct == default)
				await relay.PulseAsync(onDuration);
			else
				await relay.PulseAsync(onDuration, ct);

			await offduration.TryWaitAsync(ct);
		}
	}
}
