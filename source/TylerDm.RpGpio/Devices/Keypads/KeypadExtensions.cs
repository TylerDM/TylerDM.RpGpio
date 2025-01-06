namespace TylerDm.RpGpio.Devices.Keypads;

public static class KeypadExtensions
{
	internal const PinReadModes _pinReadMode = PinReadModes.InputPullUp;

	public static Keypad OpenKeypad(this IGpio gpio, KeypadMap map, params IEnumerable<PinNumber> pinNumbers)
	{
		var pins = pinNumbers.Select(pin => gpio.OpenRead(pin, _pinReadMode));
		return new(map, pins);
	}

	public static Keypad OpenParallax4x4(this IGpio gpio, PinNumber p0, PinNumber p1, PinNumber p2, PinNumber p3, PinNumber p4, PinNumber p5, PinNumber p6, PinNumber p7) =>
		gpio.OpenParallax4x4(
			gpio.OpenRead(p0, _pinReadMode),
			gpio.OpenRead(p1, _pinReadMode),
			gpio.OpenRead(p2, _pinReadMode),
			gpio.OpenRead(p3, _pinReadMode),
			gpio.OpenRead(p4, _pinReadMode),
			gpio.OpenRead(p5, _pinReadMode),
			gpio.OpenRead(p6, _pinReadMode),
			gpio.OpenRead(p7, _pinReadMode)
		);

	public static Keypad OpenParallax4x4(this IGpio _, IPinReader p0, IPinReader p1, IPinReader p2, IPinReader p3, IPinReader p4, IPinReader p5, IPinReader p6, IPinReader p7)
	{
		var pins = new IPinReader[] { p0, p1, p2, p3, p4, p5, p6, p7 };
		var map = buildParallax4x4Map();
		return new(map, pins);
	}

	public static async Task<string> ReadUntilAsync(this Keypad keypad, CancellationToken ct, Action<char>? onKeyPress = null)
	{
		var list = new List<char>(10);
		void handleKeyPressed(char ch)
		{
			list.Add(ch);
			onKeyPress?.Invoke(ch);
		}

		try
		{
			keypad.OnKeyPressed += handleKeyPressed;
			await ct.WaitForCancelAsync();
			return new([.. list]);
		}
		finally
		{
			keypad.OnKeyPressed -= handleKeyPressed;
		}
	}

	/// <summary>
	/// Reads characters from the keyboard until an end condition is met.
	/// </summary>
	/// <param name="count">The maximum number of characters before returning.</param>
	/// <param name="cancelChar">A character, typically #, that triggers a return.  This character is stripped from the output.</param>
	/// <param name="ct">A cancellation token to trigger return with.</param>
	/// <returns>A string containing the pressed characters.</returns>
	public static async Task<string> ReadUntilAsync(this Keypad keypad, int count, char cancelChar = default, CancellationToken ct = default)
	{
		using var cts = createIfDefaultOrLink(ct);
		var counted = 0;
		void onKeyPress(char ch)
		{
			if (ch == cancelChar || ++counted >= count)
				cts.Cancel();
		}
		var value = await keypad.ReadUntilAsync(cts.Token, onKeyPress);

		//Remove the cancelChar if it was used to cancel.
		if (value[^1] == cancelChar)
			value = value[..^1];

		return value;
	}

	private static CancellationTokenSource createIfDefaultOrLink(CancellationToken ct) =>
		ct == default ?
			new() :
			CancellationTokenSource.CreateLinkedTokenSource(ct);

	private static KeypadMap buildParallax4x4Map() =>
		new
		(
			new(7, 3, '1'), new(7, 2, '2'), new(7, 1, '3'), new(7, 0, 'A'),
			new(6, 3, '4'), new(6, 2, '5'), new(6, 1, '6'), new(6, 0, 'B'),
			new(5, 3, '7'), new(5, 2, '8'), new(5, 1, '9'), new(5, 0, 'C'),
			new(4, 3, '*'), new(4, 2, '0'), new(4, 1, '#'), new(4, 0, 'D')
		);
}
