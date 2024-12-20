namespace TylerDm.RpGpio;

public class ToggleScope : IDisposable
{
	private readonly IPinWriter _pin;

	public ToggleScope(IPinWriter pin)
	{
		_pin = pin;
		_pin.Write(true);
	}

	public void Dispose()
	{
		_pin.Write(false);
	}
}