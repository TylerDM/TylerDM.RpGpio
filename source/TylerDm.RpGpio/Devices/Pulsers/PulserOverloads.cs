namespace TylerDm.RpGpio.Devices.Pulsers;

public static class PulserOverloads
{
	public static async Task<bool> TryWaitForPulsesAsync(this Pulser pulser, int count, TimeSpan timeout)
	{
		using var timeoutCts = new Cts(timeout);
		return await pulser.TryWaitForPulsesAsync(count, timeoutCts.Token);
	}

	public static async Task<bool> TryWaitForPulsesAsync(this Pulser pulser, int count, Ct ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		for (int i = 0; i < count; i++)
		{
			var completed = await pulser.TryWaitForPulseAsync(ct);
			if (completed == false) return false;
		}
		return true;
	}

	public static async Task WaitForPulsesAsync(this Pulser pulser, int count, TimeSpan timeout)
	{
		using var timeoutCts = new Cts(timeout);
		await pulser.WaitForPulsesAsync(count, timeoutCts.Token);
	}

	public static async Task WaitForPulsesAsync(this Pulser pulser, int count, Ct ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		var result = await pulser.TryWaitForPulsesAsync(count, ct);
		if (result == false)
			throw new TaskCanceledException();
	}

	public static async Task WaitForPulseAsync(this Pulser pulser, Ct ct)
	{
		var completed = await pulser.TryWaitForPulseAsync(ct);
		if (completed == false)
			throw new TaskCanceledException();
	}
}
