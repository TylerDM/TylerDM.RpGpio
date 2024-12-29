namespace TylerDm.RpGpio.Tools;

/// <summary>
/// Tracks if an object was disposed yet or not.
/// </summary>
/// <typeparam name="T">The consuming type.  Used in ObjectDisposedException.</typeparam>
internal struct DisposedTracker<T>()
{
	private bool disposed = false;

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

		return false;
	}

	public static implicit operator bool(DisposedTracker<T> tracker) =>
		tracker.disposed;
}
