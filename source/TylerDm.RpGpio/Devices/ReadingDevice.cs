namespace TylerDm.RpGpio.Devices;

public abstract class ReadingDevice : IDevice, IDisposable
{
	private readonly IPinReader _pin;

	public PinNumber PinNumber => _pin.Number;

	protected ReadingDevice(IPinReader pin)
	{
		_pin = pin;
	}

	public void Dispose()
	{
		_pin.Dispose();
	}
}
