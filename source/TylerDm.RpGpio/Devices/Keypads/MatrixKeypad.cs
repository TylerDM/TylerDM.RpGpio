namespace TylerDm.RpGpio.Devices.Keypads;

public class MatrixKeypad : IDisposable, IKeypad
{
	private readonly DisposedTracker<MatrixKeypad> _disposed = new();
	private readonly IPinWriter[] _writingPins;
	private readonly MatrixKeypadReadingPin[] _readingPins;
	private readonly char[,] _mapping;

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

	/// <summary>
	/// The delay after a key press before allowing another key press.  This prevents double key presses and other electrical noise.
	/// </summary>
	public TimeSpan NoiseDelay { get; set; } = TimeSpan.FromMilliseconds(250);

	public MatrixKeypad(IPinReader[] readingPins, IPinWriter[] writingPins, char[,] mapping)
	{
		_readingPins = readingPins.Select(x => new MatrixKeypadReadingPin(x)).ToArray();
		foreach (var readingPin in _readingPins)
			readingPin.OnActivated += handlePinRead;

		_writingPins = writingPins;
		_mapping = mapping;

		writeAll(true);
	}

	public void Dispose()
	{
		if (_disposed.Dispose()) return;

		foreach (var writingPin in _writingPins)
			writingPin.Dispose();
		foreach (var readingPin in _readingPins)
		{
			readingPin.OnActivated -= handlePinRead;
			readingPin.Dispose();
		}
	}

	private void handlePinRead(MatrixKeypadReadingPin readingPin)
	{
		setListeningAll(false);

		try
		{
			var readingPinIndex = _readingPins.IndexOf(readingPin);
			if (readingPinIndex is null) return;
			var writingPinIndex = getWritingPinIndex(readingPin);
			if (writingPinIndex is null) return;

			var ch = getPressedChar(readingPinIndex.Value, writingPinIndex.Value);
			onKeyPressed?.Invoke(ch);
		}
		finally
		{
			resumeListening();
		}
	}

	private async void resumeListening()
	{
		await NoiseDelay.WaitAsync();
		setListeningAll(true);
	}

	private int? getWritingPinIndex(MatrixKeypadReadingPin readingPin)
	{
		if (readingPin.Read() == false) return null;

		try
		{
			for (var i = 0; i < _writingPins.Length; i++)
			{
				var writingPin = _writingPins[i];
				writingPin.Write(false);
				if (readingPin.Read() == false) return i;
			}

			return null;
		}
		finally
		{
			writeAll(true);
		}
	}

	private void setListeningAll(bool value) =>
		_readingPins.ForEach(x => x.Listening = value);

	private void writeAll(bool value) =>
		_writingPins.ForEach(x => x.Write(value));

	private char getPressedChar(int readingPinIndex, int writingPinIndex)
	{
		try
		{
			var result = _mapping[readingPinIndex, writingPinIndex];
			if (result == default)
				throw new Exception("Mapping returned default value for given combination of pins.");
			return result;
		}
		catch (IndexOutOfRangeException)
		{
			throw new Exception("Key map does not contain a value for the given combination of writing and reading pins.");
		}
	}
}
