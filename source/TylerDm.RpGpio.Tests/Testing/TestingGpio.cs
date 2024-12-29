namespace TylerDm.RpGpio.Testing;

public class TestingGpio : IGpio
{
	private readonly Dictionary<PinNumber, bool> _values = [];

	public IPinReader OpenRead(PinNumber pinNumber, PinReadModes mode = PinReadModes.Input) =>
		new TestingPin(this, pinNumber, mode);//Mode is irrelevant.

	public IPinWriter OpenWrite(PinNumber pinNumber) =>
		new TestingPin(this, pinNumber, PinReadModes.Input);

	internal void Write(PinNumber number, bool value) =>
		_values[number] = value;

	internal bool Read(PinNumber number) =>
		_values.TryGetValue(number, out bool value) && value;
}
