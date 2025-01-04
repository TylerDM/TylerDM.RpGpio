namespace TylerDm.RpGpio.Testing;

public class TestingPin : IPinReader, IPinWriter
{
	private readonly bool _restValue;

	private PinChangedEvent? valueChanged;
	private bool value;

	public event PinChangedEvent ValueChanged
	{
		add => valueChanged += value;
		remove => valueChanged -= value;
	}

	public PinNumber Number { get; }
	public PinReadModes Mode { get; }

	public TestingPin(PinNumber number, PinReadModes mode)
	{
		Number = number;
		Mode = mode;
		//Pull up resistors will cause the value to read high by default.
		_restValue = mode == PinReadModes.InputPullUp;
		value = _restValue;
	}

	public void Dispose() { }

	public bool Read() =>
		value;

	public void Write(bool newValue)
	{
		var previousValue = Read();

		value = newValue;

		if (valueChanged is null) return;

		var eventType = getEventType(previousValue, newValue);
		valueChanged.Invoke(eventType);
	}

	public void Pulse(bool? value = null)
	{
		value ??= Mode != PinReadModes.InputPullUp;

		Write(value.Value);
		Write(!value.Value);
	}

	private static PinEventTypes getEventType(bool previousValue, bool newValue)
	{
		if (previousValue == newValue) return PinEventTypes.None;
		return newValue ? PinEventTypes.Rising : PinEventTypes.Falling;
	}
}
