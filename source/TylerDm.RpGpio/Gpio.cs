namespace TylerDm.RpGpio;

[DependencyInjectable(ServiceLifetime.Singleton, typeof(IGpio))]
public class Gpio : IDisposable, IGpio
{
	private readonly GpioController _controller = new();

	public IPinWriter OpenWrite(PinNumber pinNumber)
	{
		var pin = _controller.OpenPin((int)pinNumber, PinMode.Output);
		return new PinWriter(_controller, pin);
	}

	public IPinReader OpenRead(PinNumber pinNumber)
	{
		var pin = _controller.OpenPin((int)pinNumber, PinMode.Input);
		return new PinReader(_controller, pin);
	}

	public void Dispose() =>
		_controller.Dispose();
}
