namespace TylerDm.RpGpio.Extensions;

public static class TimeSpanExt
{
	public static Task WaitAsync(this TimeSpan duration, CancellationToken ct) =>
		Task.Delay(duration, ct);
}
