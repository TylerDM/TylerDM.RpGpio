namespace TylerDm.RpGpio.Extensions;

public static class CtExt
{
	internal static Task WaitForCancelAsync(this Ct ct)
	{
		var tcs = new TaskCompletionSource();
		ct.Register(() => tcs.TrySetResult());
		return tcs.Task;
	}
}
