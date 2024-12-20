namespace TylerDm.RpGpio;

public static class PinReaderExt
{
	public static async Task WaitForPulsesAsync(this IPinReader reader, int count, TimeSpan maxWait)
	{
		using var cts = new CancellationTokenSource();
		cts.CancelAfter(maxWait);

		await reader.WaitForPulsesAsync(count, cts.Token);
	}

	public static async Task WaitForPulsesAsync(this IPinReader reader, int count, CancellationToken ct)
	{
		var pulses = 0;
		var gate = new Gate();

		void handlePulse(PinEvents type)
		{
			if (type is not PinEvents.Rising) return;

			pulses++;
			if (pulses >= count)
				gate.Release();
		}

		try
		{
			reader.ValueChanged += handlePulse;
			await gate.TryWaitAsync(ct);
		}
		finally
		{
			reader.ValueChanged -= handlePulse;
		}
	}

	public static async Task<decimal> CountPulsesAsync(this IPinReader reader, CancellationToken ct, Action? onPulse = null)
	{
		var pulses = 0;
		var gate = new Gate();

		void handlePulse(PinEvents type)
		{
			if (type is not PinEvents.Rising) return;

			pulses++;
			onPulse?.Invoke();
		}

		try
		{
			reader.ValueChanged += handlePulse;
			await gate.TryWaitAsync(ct);
			return pulses;
		}
		finally
		{
			reader.ValueChanged -= handlePulse;
		}
	}
}
