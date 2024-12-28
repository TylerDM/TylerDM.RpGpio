namespace TylerDm.RpGpio.Extensions;

/// <summary>
/// Tracks if an object was disposed yet or not.
/// </summary>
/// <typeparam name="T">The consuming type.  Used in ObjectDisposedException.</typeparam>
internal struct DisposedTracker<T>()
{
	private readonly Cts _cts = new();

	private bool disposed = false;

	public readonly Ct CancellationToken => _cts.Token;

	public readonly Cts CreateLinkedCts(Cts cts) =>
		CreateLinkedCts(_cts.Token);

	public readonly Cts CreateLinkedCts(Ct token) =>
		Cts.CreateLinkedTokenSource(_cts.Token, token);

	public readonly void ThrowIf() =>
		ObjectDisposedException.ThrowIf(disposed, typeof(T));

	/// <summary>
	/// Disposes the tracker if needed. Consumers should short circuit if this method returns true.
	/// </summary>
	/// <returns>Returns if Dispose() was called previously.</returns>
	public bool Dispose()
	{
		if (disposed) return true;
		disposed = true;

		_cts.Dispose();
		return false;
	}

	public static implicit operator bool(DisposedTracker<T> tracker) =>
		tracker.disposed;
}
