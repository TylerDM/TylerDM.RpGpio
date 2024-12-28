namespace TylerDm.RpGpio.Devices;

public abstract class ReadingDevice : IDevice, IDisposable
{
	private readonly IPinReader _pin;

	public PinNumber PinNumber => _pin.Number;

	protected ReadingDevice(IPinReader pin)
	{
		_pin = pin;
		_pin.ValueChanged += handleValueChanged;
	}

	public virtual void Dispose()
	{
		_pin.ValueChanged -= handleValueChanged;
		_pin.Dispose();
	}

	protected abstract void handleValueChanged(PinEventTypes type);
}
