namespace TylerDm.RpGpio.Devices;

public abstract class WritingDevice(IPinWriter pin) : IDevice, IDisposable
{
	private readonly IPinWriter _pin = pin;

	private bool state;
	private bool disposed;

	public PinNumber PinNumber => _pin.Number;

	public virtual void Dispose()
	{
		if (disposed) return;
		disposed = true;

		_pin.Dispose();
	}

	protected bool getValue()
	{
		throwIfDisposed();

		return state;
	}

	protected void setValue(bool value)
	{
		throwIfDisposed();

		_pin.Write(value);
		state = value;
	}

	private void throwIfDisposed() =>
		ObjectDisposedException.ThrowIf(disposed, GetType());
}
