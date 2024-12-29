namespace TylerDm.RpGpio.Tools;

public class BgTaskHandle : IDisposable
{
	private readonly CancellationTokenSource _cts = new();

	public BgTaskHandle(Func<CancellationToken, Task> func) =>
		Task.Run(() => func(_cts.Token));

	public void Dispose() =>
		_cts.Dispose();
}
