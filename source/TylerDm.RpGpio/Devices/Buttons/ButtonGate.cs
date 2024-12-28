namespace TylerDm.RpGpio.Devices.Buttons;

public class ButtonGate : IDisposable
{
	internal readonly DisposedTracker<ButtonGate> _disposed = new();
	private readonly Cts _cts;
	private readonly Task<bool> _waitTask;

	internal ButtonGate(Button button)
	{
		_cts = new();
		_waitTask = button.TryWaitForPressAsync(_cts.Token);
	}

	#region wait methods
	public async Task<bool> TryWaitAsync(TimeSpan timeSpan)
	{
		_disposed.ThrowIf();

		using var timedCts = new Cts(timeSpan);
		return await TryWaitAsync(timedCts.Token);
	}

	public async Task<bool> TryWaitAsync(Ct ct)
	{
		_disposed.ThrowIf();

		using var linkedCts = _cts.CreateLinked(ct);
		return await _waitTask.TryWaitAsync(linkedCts);
	}

	public async Task<bool> TryWaitAsync()
	{
		_disposed.ThrowIf();
		return await _waitTask.TryWaitAsync(_cts);
	}

	public async Task WaitAsync(TimeSpan timeSpan)
	{
		_disposed.ThrowIf();

		using var timedCts = new Cts(timeSpan);
		await WaitAsync(timedCts.Token);
	}

	public async Task WaitAsync(Ct ct)
	{
		_disposed.ThrowIf();

		using var linkedCts = _cts.CreateLinked(ct);
		await waitWithThrowAsync(_cts);
	}

	public async Task WaitAsync()
	{
		_disposed.ThrowIf();
		await waitWithThrowAsync(_cts);
	}
	#endregion

	public void Dispose()
	{
		if (_disposed) return;
		_disposed.Dispose();

		_cts.Dispose();
	}

	private Task waitWithThrowAsync(Cts cts) =>
		waitWithThrowAsync(cts.Token);

	//This method is necessary as _waitTask is produced by TryWaitForPressAsync, which does not throw on cancellation.  But callers may want, and should expect, that behavior from WaitAsync().
	private async Task waitWithThrowAsync(Ct ct)
	{
		var completed = await _waitTask.WaitAsync(ct);
		if (completed == false)
			throw new TaskCanceledException();
	}
}
