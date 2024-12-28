namespace TylerDm.RpGpio.Extensions;

public static class CtsExt
{
	public static Cts CreateLinked(this Cts root, Cts cts) =>
		root.Token.CreateLinked(cts.Token);

	public static Cts CreateLinked(this Cts root, Ct ct) =>
		root.Token.CreateLinked(ct);

	public static Cts CreateLinked(this Ct root, Ct ct) =>
		Cts.CreateLinkedTokenSource(root, ct);
}
