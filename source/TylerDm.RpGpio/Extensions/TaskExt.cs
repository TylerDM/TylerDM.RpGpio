namespace TylerDm.RpGpio.Extensions;

public static class TaskExt
{
	public static async Task<bool> TryWaitAsync(this Task task, TimeSpan timeout)
	{
		using var cts = new CancellationTokenSource(timeout);
		return await task.TryWaitAsync(cts.Token);
	}

	public static async Task<bool> TryWaitAsync(this Task task, CancellationToken cancellationToken)
	{
		try
		{
			await task.WaitAsync(cancellationToken);
			return true;
		}
		catch (OperationCanceledException)
		{
			return false;
		}
	}
}
