namespace TylerDm.RpGpio.Extensions;

public static class TimeSpanExt
{
	public static Task<bool> TryWaitAsync(this TimeSpan duration, Cts cts) =>
		duration.TryWaitAsync(cts.Token);
}
