namespace TylerDm.RpGpio;

[DependencyInjectable(ServiceLifetime.Singleton, typeof(IGpio))]
public class Gpio : IDisposable, IGpio
{
	private readonly GpioController _controller = new();

	public PinWriter OpenWrite(PinNumber pinNumber)
	{
		var pin = _controller.OpenPin((int)pinNumber, PinMode.Output);
		return new PinWriter(_controller, pin);
	}

	public PinReader OpenRead(PinNumber pinNumber)
	{
		var pin = _controller.OpenPin((int)pinNumber, PinMode.InputPullDown);
		return new PinReader(_controller, pin);
	}

	public void Dispose() =>
		_controller.Dispose();

	IPinWriter IGpio.OpenWrite(PinNumber pinNumber) =>
		OpenWrite(pinNumber);

	IPinReader IGpio.OpenRead(PinNumber pinNumber) =>
		OpenRead(pinNumber);
}
