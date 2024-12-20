namespace TylerDm.RpGpio;

public interface IPinReader : IPin
{
	event PinChangedEvent ValueChanged;

	bool Read();

	public async Task WaitForPulsesAsync(int count, TimeSpan maxWait)
	{
		using var cts = new CancellationTokenSource();
		cts.CancelAfter(maxWait);

		await WaitForPulsesAsync(count, cts.Token);
	}

	public async Task WaitForPulsesAsync(int count, CancellationToken ct)
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
			ValueChanged += handlePulse;
			await gate.TryWaitAsync(ct);
		}
		finally
		{
			ValueChanged -= handlePulse;
		}
	}

	public async Task<decimal> CountPulsesAsync(CancellationToken ct, Action? onPulse = null)
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
			ValueChanged += handlePulse;
			await gate.TryWaitAsync(ct);
			return pulses;
		}
		finally
		{
			ValueChanged -= handlePulse;
		}
	}
}
