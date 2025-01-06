namespace TylerDm.RpGpio.Devices.Keypads;

public class Keypad : IDisposable
{
	private const PinEventTypes _pressedEventType = PinEventTypes.Falling;

	private readonly DisposedTracker<Keypad> _disposed = new();
	private readonly Lock _lock = new();

	private readonly IPinReader[] _pins;
	private readonly KeypadMap _map;

	private int? firstPinPressed;
	private DateTime? firstPinPressedWhen;

	private event KeypadEvent? onKeyPressed;

	public event KeypadEvent OnKeyPressed
	{
		add
		{
			_disposed.ThrowIf();

			onKeyPressed += value;
		}
		remove => onKeyPressed -= value;
	}

	internal Keypad(KeypadMap map, params IEnumerable<IPinReader> pins)
	{
		_pins = pins.ToArray();
		_pins.ForEach(x => x.ValueChanged += eventType => handleValueChanged(x, eventType));
		_map = map;
	}

	public void Dispose()
	{
		if (_disposed.Dispose()) return;

		_pins.ForEach(x => x.Dispose());
		onKeyPressed = null;
	}

	private void handleValueChanged(IPinReader reader, PinEventTypes type)
	{
		lock (_lock)
		{
			if (type != _pressedEventType)
				return;

			clearStaleFirstPin();

			var pinPressed = _pins.IndexOfRequired(reader);

			if (firstPinPressed is null)
			{
				setFirstPin(pinPressed);
				return;
			}

			var keyPressed = _map.GetChar(firstPinPressed.Value, pinPressed);
			if (keyPressed is null) return;//Unmapped or invalid input.

			onKeyPressed?.Invoke(keyPressed.Value);
			clearFirstPin();
		}
	}

	private void clearStaleFirstPin()
	{
		if (getIsFirstPinValid()) return;

		clearFirstPin();
	}

	private bool getIsFirstPinValid()
	{
		if (firstPinPressedWhen is not DateTime when) return true;

		//If the last pin input was too long ago, we need to ignore it as noise.
		var _1msAgo = DateTime.UtcNow.AddMilliseconds(-1);
		return _1msAgo < when;
	}

	private void setFirstPin(int pin)
	{
		firstPinPressed = pin;
		firstPinPressedWhen = DateTime.UtcNow;
	}

	private void clearFirstPin()
	{
		firstPinPressed = null;
		firstPinPressedWhen = null;
	}
}
