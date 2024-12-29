namespace TylerDm.RpGpio.Tools;

public class BgTaskHandle : IDisposable
{
	private readonly CancellationTokenSource _cts = new();
	private readonly Task _task;

	private bool disposed = false;

	public BgTaskHandle(Func<CancellationToken, Task> func) =>
		_task = Task.Run(() => func(_cts.Token));

	public BgTaskHandle(Action<CancellationToken> action) =>
		_task = Task.Run(() => action(_cts.Token));

	/// <summary>
	/// Wait for the task to complete.
	/// </summary>
	public Task WaitAsync(CancellationToken ct = default) =>
		_task.WaitAsync(ct);

	/// <summary>
	/// Cancels the background task and disposes unmanaged internal resources.
	/// </summary>
	public void Dispose()
	{
		if (disposed) return;
		disposed = true;

		_cts.Dispose();
	}
}
