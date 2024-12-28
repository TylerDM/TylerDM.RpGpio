namespace TylerDm.RpGpio.Devices;

public abstract class WritingDevice : IDevice, IDisposable
{
	private readonly IPinWriter _pin;

	private bool state;
	private bool disposed;

	public PinNumber PinNumber => _pin.Number;

	protected WritingDevice(IPinWriter pin)
	{
		_pin = pin;
	}

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
