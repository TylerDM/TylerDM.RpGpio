namespace TylerDm.RpGpio.Testing;

[DependencyInjectable(ServiceLifetime.Singleton, typeof(IGpio))]
public class TestingGpio : IGpio
{
	private readonly Dictionary<PinNumber, bool> _values = [];

	public IPinReader OpenRead(PinNumber pinNumber) =>
		new TestingPin(this, pinNumber);

	public IPinWriter OpenWrite(PinNumber pinNumber) =>
		new TestingPin(this, pinNumber);

	internal void Write(PinNumber number, bool value) =>
		_values[number] = value;

	internal bool Read(PinNumber number) =>
		_values.TryGetValue(number, out bool value) && value;
}
