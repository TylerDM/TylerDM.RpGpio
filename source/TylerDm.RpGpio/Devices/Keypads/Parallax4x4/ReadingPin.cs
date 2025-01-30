namespace TylerDm.RpGpio.Devices.Keypads.Parallax4x4;

internal class ReadingPin : IDisposable
{
	private const PinEventTypes _activatingEvent = PinEventTypes.Rising;

	private readonly DisposedTracker<ReadingPin> _disposed = new();

	private readonly IPinReader _pin;

	private bool raised = false;

	private event ReadingPinActivatedEvent? onActivated;

	public event ReadingPinActivatedEvent OnActivated
	{
		add => onActivated += value;
		remove => onActivated -= value;
	}
	public bool Listening { get; set; } = true;
	public ReadingPins PinNumber { get; }

	public ReadingPin(IPinReader pin, ReadingPins pinNumber)
	{
		_pin = pin;
		PinNumber = pinNumber;
		_pin.ValueChanged += handlePinChanged;
	}

	public bool Read() =>
		_pin.Read();

	public void Dispose()
	{
		if (_disposed.Dispose()) return;

		_pin.ValueChanged -= handlePinChanged;
		_pin.Dispose();
	}

	private void handlePinChanged(PinEventTypes eventType)
	{
		if (eventType == PinEventTypes.None) return;

		if (eventType != _activatingEvent)
		{
			raised = false;
			return;
		}

		//Prevent multiple raise events.
		if (raised) return;
		raised = true;

		if (Listening == false) return;

		onActivated?.Invoke(this);
	}
}
