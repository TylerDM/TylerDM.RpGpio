namespace TylerDm.RpGpio.Extensions;

public static class TimeSpanExt
{
	public static Task<bool> TryWaitAsync(this TimeSpan duration, Cts cts) =>
		duration.TryWaitAsync(cts.Token);

	public static Task WaitAsync(this TimeSpan duration, Cts cts) =>
		duration.WaitAsync(cts.Token);

	public static Task WaitAsync(this TimeSpan duration, Ct ct) =>
		Task.Delay(duration, ct);
}
