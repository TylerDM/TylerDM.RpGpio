namespace TylerDm.RpGpio.Testing;

public class TestingPin(TestingGpio gpio, PinNumber number, PinReadModes mode) : IPinReader, IPinWriter
{
	private PinChangedEvent? valueChanged;

	public event PinChangedEvent ValueChanged
	{
		add => valueChanged += value;
		remove => valueChanged -= value;
	}

	public PinNumber Number { get; } = number;
	public PinReadModes Mode { get; } = mode;

	public void Dispose() { }

	public bool Read() =>
		gpio.Read(Number);

	public void Write(bool newValue)
	{
		var previousValue = Read();

		gpio.Write(Number, newValue);

		if (valueChanged is null) return;

		var eventType = getEventType(previousValue, newValue);
		valueChanged.Invoke(eventType);
	}

	private static PinEventTypes getEventType(bool previousValue, bool newValue)
	{
		if (previousValue == newValue) return PinEventTypes.None;
		return newValue ? PinEventTypes.Rising : PinEventTypes.Falling;
	}
}
