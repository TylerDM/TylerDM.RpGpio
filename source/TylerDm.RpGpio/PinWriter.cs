namespace TylerDm.RpGpio;

public class PinWriter(GpioController _controller, GpioPin _pin) : Pin(_controller, _pin), IPinWriter
{
	public void Write(bool value) =>
		_pin.Write(value);
}
