namespace TylerDm.RpGpio.Devices.Keypads;

internal class MatrixKeypadReadingPin : IDisposable
{
	private const PinEventTypes _activatingEvent = PinEventTypes.Rising;

	private readonly DisposedTracker<MatrixKeypadReadingPin> _disposed = new();

	private readonly IPinReader _pin;

	private bool raised = false;

	private event MatrixKeypadReadingPinActivated? onActivated;

	public event MatrixKeypadReadingPinActivated OnActivated
	{
		add => onActivated += value;
		remove => onActivated -= value;
	}
	public bool Listening { get; set; } = true;

	public MatrixKeypadReadingPin(IPinReader pin)
	{
		_pin = pin;
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
