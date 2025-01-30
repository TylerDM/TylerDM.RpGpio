using System.Device.Gpio.Drivers;

namespace TylerDm.RpGpio;

public class Gpio(GpioController? controller = null) : IDisposable, IGpio
{
	private readonly GpioController _controller = controller ?? new();

	public Gpio(int chipNumber, PinNumberingScheme scheme = PinNumberingScheme.Logical) : this(new
		(
			scheme,
			new LibGpiodDriver(chipNumber)
		))
	{ }

	public PinWriter OpenWrite(PinNumber pinNumber)
	{
		var pin = _controller.OpenPin((int)pinNumber, PinMode.Output);
		return new(_controller, pin);
	}

	public PinReader OpenRead(PinNumber pinNumber, PinReadModes mode = PinReadModes.InputPullDown)
	{
		var pin = _controller.OpenPin((int)pinNumber, (PinMode)mode);
		return new(_controller, pin);
	}

	public void Dispose() =>
		_controller.Dispose();

	IPinWriter IGpio.OpenWrite(PinNumber pinNumber) =>
		OpenWrite(pinNumber);

	IPinReader IGpio.OpenRead(PinNumber pinNumber, PinReadModes mode) =>
		OpenRead(pinNumber, mode);
}
