namespace TylerDm.RpGpio;

public static class PinReaderExt
{
	/// <summary>
	/// Holds the thread until the specified number of pulses have been read.
	/// </summary>
	public static async Task WaitForPulsesAsync(this IPinReader reader, int count, TimeSpan timeout)
	{
		using var cts = new CancellationTokenSource(timeout);
		await reader.WaitForPulsesAsync(count, cts.Token);
	}

	/// <summary>
	/// Holds the thread until the specified number of pulses have been read.
	/// </summary>
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
			await gate.WaitAsync(ct);
		}
		finally
		{
			reader.ValueChanged -= handlePulse;
		}
	}

	/// <summary>
	/// Counts the number of pulses between invocation and timeout. Executes onPulse each time a pulse of observed.
	/// </summary>
	public static async Task<decimal> CountPulsesAsync(this IPinReader reader, TimeSpan timeout, Action? onPulse = null)
	{
		using var cts = new CancellationTokenSource(timeout);
		return await reader.CountPulsesAsync(cts.Token, onPulse);
	}

	/// <summary>
	/// Counts the number of pulses between invocation and cancellation. Executes onPulse each time a pulse of observed.
	/// </summary>
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
