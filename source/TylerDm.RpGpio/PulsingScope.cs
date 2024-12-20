namespace TylerDm.RpGpio;

public class PulsingScope : IAsyncDisposable
{
	private static readonly TimeSpan _defaultPulsingOnTime = TimeSpan.FromMilliseconds(500);
	private static readonly TimeSpan _defaultPulsingOffTime = TimeSpan.FromMilliseconds(500);

	private readonly CancellationTokenSource _cts = new();
	private readonly IPinWriter _pin;
	private readonly TimeSpan _pulsingOnTime;
	private readonly TimeSpan _pulsingOffTime;
	private readonly Task _pulsingTask;

	public PulsingScope(IPinWriter pin, TimeSpan? onTime = null, TimeSpan? offTime = null)
	{
		_pin = pin;
		_pulsingOnTime = onTime ?? _defaultPulsingOnTime;
		_pulsingOffTime = offTime ?? _defaultPulsingOffTime;
		_pulsingTask = Task.Run(pulseAsync);
	}

	public async ValueTask DisposeAsync()
	{
		_cts.Cancel();
		await _pulsingTask;
		_cts.Dispose();
	}

	private async Task pulseAsync()
	{
		var ct = _cts.Token;
		while (ct.ShouldContinue())
		{
			_pin.Write(true);
			await _pulsingOnTime.TryWaitAsync(ct);

			_pin.Write(false);
			await _pulsingOffTime.TryWaitAsync(ct);
		}
		_pin.Write(false);
	}
}
