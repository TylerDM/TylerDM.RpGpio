namespace TylerDm.RpGpio.Devices.Pulsers;

public static class PulserExtensions
{
	public static Pulser OpenPulser(this IGpio gpio, PinNumber pin, PinReadModes mode = PinReadModes.InputPullDown) =>
		new(gpio.OpenRead(pin, mode));

	public static async Task WaitForPulsesAsync(this Pulser pulser, int count, TimeSpan minimumDuration = default, CancellationToken ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

		for (int i = 0; i < count; i++)
		{
			if (ct.IsCancellationRequested) break;

			await pulser.WaitForPulseAsync(minimumDuration, ct);
		}
	}

	public static async Task WaitForPulseAsync(this Pulser pulser, TimeSpan minimumDuration = default, CancellationToken ct = default)
	{
		using var gate = new Gate();
		void handlePulse(TimeSpan duration)
		{
			if (duration <= minimumDuration) return;
			gate.Open();
		}
		try
		{
			pulser.OnPulseEnded += handlePulse;
			await gate.WaitAsync(ct);
		}
		finally
		{
			pulser.OnPulseEnded -= handlePulse;
		}
	}

	public static async Task<int> CountPulsesAsync(this Pulser pulser, CancellationToken ct)
	{
		var value = 0;
		using var gate = new Gate();
		void handlePulse() => value++;
		try
		{
			pulser.OnPulseStarted += handlePulse;
			await gate.TryWaitAsync(ct);
		}
		finally
		{
			pulser.OnPulseStarted -= handlePulse;
		}
		return value;
	}
}
