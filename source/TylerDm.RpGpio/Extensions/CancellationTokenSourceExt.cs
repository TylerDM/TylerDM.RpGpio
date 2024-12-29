namespace TylerDm.RpGpio.Extensions;

public static class CancellationTokenSourceExt
{
	public static CancellationTokenSource CreateLinked(this CancellationTokenSource root, CancellationToken ct) =>
		root.Token.CreateLinked(ct);

	public static CancellationTokenSource CreateLinked(this CancellationToken root, CancellationToken ct) =>
		CancellationTokenSource.CreateLinkedTokenSource(root, ct);
}
