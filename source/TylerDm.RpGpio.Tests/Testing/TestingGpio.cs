namespace TylerDm.RpGpio.Testing;

public class TestingGpio : IGpio
{
	public TestingPin OpenRead(PinNumber pinNumber, PinReadModes mode = PinReadModes.Input) =>
		new(pinNumber, mode);

	public TestingPin OpenWrite(PinNumber pinNumber) =>
		new(pinNumber, PinReadModes.Input);

	void IDisposable.Dispose()
	{
	}

	IPinWriter IGpio.OpenWrite(PinNumber pinNumber) =>
		OpenWrite(pinNumber);

	IPinReader IGpio.OpenRead(PinNumber pinNumber, PinReadModes mode) =>
		OpenRead(pinNumber, mode);
}
