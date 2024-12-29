namespace TylerDm.RpGpio.Extensions;

public static class CancellationTokenExt
{
	internal static Task WaitForCancelAsync(this CancellationToken ct)
	{
		var tcs = new TaskCompletionSource();
		ct.Register(() => tcs.TrySetResult());
		return tcs.Task;
	}
}
