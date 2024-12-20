namespace TylerDm.RpGpio;

public abstract class Pin(GpioController controller, GpioPin pin) : IPin
{
	protected GpioController _controller = controller;
	protected GpioPin _pin = pin;

	public PinNumber Number => (PinNumber)_pin.PinNumber;

	public virtual void Dispose() =>
		_controller.ClosePin(_pin.PinNumber);
}
