namespace TylerDm.RpGpio.Devices.Pulsers;

public static class PulserExtensions
{
	public static Pulser OpenPulser(this IGpio gpio, PinNumber pin, PinReadModes mode = PinReadModes.InputPullDown) =>
		new(gpio.OpenRead(pin, mode));

	public static async Task WaitForPulsesAsync(this Pulser pulser, int count, CancellationToken ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		for (int i = 0; i < count; i++)
		{
			if (ct.IsCancellationRequested) break;

			await pulser.WaitForPulseAsync(ct);
		}
	}

	public static async Task WaitForPulseAsync(this Pulser pulser, CancellationToken ct = default)
	{
		using var gate = new Gate();
		void handlePulse() => gate.Open();
		try
		{
			pulser.OnPulseStart += handlePulse;
			await gate.WaitAsync(ct);
		}
		finally
		{
			pulser.OnPulseStart -= handlePulse;
		}
	}

	public static async Task<int> CountPulsesAsync(this Pulser pulser, CancellationToken ct)
	{
		var value = 0;
		using var gate = new Gate();
		void handlePulse() => value++;
		try
		{
			pulser.OnPulseStart += handlePulse;
			await gate.TryWaitAsync(ct);
		}
		finally
		{
			pulser.OnPulseStart -= handlePulse;
		}
		return value;
	}
}
