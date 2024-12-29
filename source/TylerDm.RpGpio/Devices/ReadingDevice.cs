namespace TylerDm.RpGpio.Devices;

public abstract class ReadingDevice : IDevice, IDisposable
{
	protected readonly IPinReader _pin;
	protected readonly PinEventTypes _activatingEvent;
	protected readonly PinEventTypes _deactivatingEvent;

	public PinNumber PinNumber => _pin.Number;

	protected ReadingDevice(IPinReader pin)
	{
		_pin = pin;
		_activatingEvent = getActivatingEvent(_pin.Mode);
		_deactivatingEvent = getDeactivatingEvent(_pin.Mode);
		_pin.ValueChanged += handleValueChanged;
	}

	public virtual void Dispose()
	{
		_pin.ValueChanged -= handleValueChanged;
		_pin.Dispose();
	}

	protected abstract void handleValueChanged(PinEventTypes type);

	private static PinEventTypes getActivatingEvent(PinReadModes mode) =>
		mode is PinReadModes.InputPullUp ?
			PinEventTypes.Falling :
			PinEventTypes.Rising;

	private static PinEventTypes getDeactivatingEvent(PinReadModes mode) =>
		mode is PinReadModes.InputPullUp ?
			PinEventTypes.Rising :
			PinEventTypes.Falling;
}
