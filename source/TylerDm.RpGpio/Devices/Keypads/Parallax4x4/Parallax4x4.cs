namespace TylerDm.RpGpio.Devices.Keypads.Parallax4x4;

//Pin numbering matches document at https://cdn.sparkfun.com/assets/f/f/a/5/0/DS-16038.pdf
public class Parallax4x4 : IDisposable
{
	private static readonly TimeSpan _noiseDelay = TimeSpan.FromMilliseconds(250);

	private readonly DisposedTracker<Parallax4x4> _disposed = new();

	private readonly IPinWriter _pin0;
	private readonly IPinWriter _pin1;
	private readonly IPinWriter _pin2;
	private readonly IPinWriter _pin3;
	private readonly IPinWriter[] _writingPins;

	private readonly ReadingPin _pin4;
	private readonly ReadingPin _pin5;
	private readonly ReadingPin _pin6;
	private readonly ReadingPin _pin7;
	private readonly ReadingPin[] _readingPins;

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

	public Parallax4x4(
		IPinWriter pin0, IPinWriter pin1, IPinWriter pin2, IPinWriter pin3,
		IPinReader pin4, IPinReader pin5, IPinReader pin6, IPinReader pin7
	)
	{
		//Writers
		_pin0 = pin0;
		_pin1 = pin1;
		_pin2 = pin2;
		_pin3 = pin3;
		_writingPins = [_pin0, _pin1, _pin2, _pin3];
		writeAll(true);

		//Readers
		_pin4 = new(pin4, ReadingPins._4);
		_pin4.OnActivated += handlePinRead;
		_pin5 = new(pin5, ReadingPins._5);
		_pin5.OnActivated += handlePinRead;
		_pin6 = new(pin6, ReadingPins._6);
		_pin6.OnActivated += handlePinRead;
		_pin7 = new(pin7, ReadingPins._7);
		_pin7.OnActivated += handlePinRead;
		_readingPins = [_pin4, _pin5, _pin6, _pin7];
	}

	public void Dispose()
	{
		if (_disposed.Dispose()) return;

		_pin0.Dispose();
		_pin1.Dispose();
		_pin2.Dispose();
		_pin3.Dispose();

		_pin4.Dispose();
		_pin5.Dispose();
		_pin6.Dispose();
		_pin7.Dispose();
	}

	private void handlePinRead(ReadingPin readingPin)
	{
		setListeningAll(false);

		try
		{
			var writingPinNumber = getWritingPinNumber(readingPin);
			if (writingPinNumber is null)
				return;

			var ch = getPressedChar(readingPin.PinNumber, writingPinNumber.Value);
			onKeyPressed?.Invoke(ch);
		}
		finally
		{
			resumeListeningLaterAsync();
		}
	}

	private async void resumeListeningLaterAsync()
	{
		await _noiseDelay.WaitAsync();
		setListeningAll(true);
	}

	private WritingPins? getWritingPinNumber(ReadingPin readingPin)
	{
		if (readingPin.Read() == false) return null;

		try
		{
			_pin0.Write(false);
			if (readingPin.Read() == false) return WritingPins._0;

			_pin1.Write(false);
			if (readingPin.Read() == false) return WritingPins._1;

			_pin2.Write(false);
			if (readingPin.Read() == false) return WritingPins._2;

			_pin3.Write(false);
			if (readingPin.Read() == false) return WritingPins._3;

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

	private static char getPressedChar(ReadingPins readingPin, WritingPins writingPin) =>
		(readingPin, writingPin) switch
		{
			(ReadingPins._7, WritingPins._3) => '1',
			(ReadingPins._7, WritingPins._2) => '2',
			(ReadingPins._7, WritingPins._1) => '3',
			(ReadingPins._7, WritingPins._0) => 'A',

			(ReadingPins._6, WritingPins._3) => '4',
			(ReadingPins._6, WritingPins._2) => '5',
			(ReadingPins._6, WritingPins._1) => '6',
			(ReadingPins._6, WritingPins._0) => 'B',

			(ReadingPins._5, WritingPins._3) => '7',
			(ReadingPins._5, WritingPins._2) => '8',
			(ReadingPins._5, WritingPins._1) => '9',
			(ReadingPins._5, WritingPins._0) => 'C',

			(ReadingPins._4, WritingPins._3) => '*',
			(ReadingPins._4, WritingPins._2) => '0',
			(ReadingPins._4, WritingPins._1) => '#',
			(ReadingPins._4, WritingPins._0) => 'D',

			_ => throw new Exception("Invalid combination of pins.")
		};
}
