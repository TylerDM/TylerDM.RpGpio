namespace TylerDm.RpGpio.Devices.Keypads;

public static class KeypadExtensions
{
	/// <summary>
	/// Opens a MatrixKeypad object.
	/// </summary>
	/// <param name="gpio">The GPIO bus to use.</param>
	/// <param name="readingPins">The reading pins to use.  It is recommended you use your reading pins as rows to make the mapping matrix intuitive.  But, you may use them as columns.</param>
	/// <param name="writingPins">The writing pins to use.  It is recommended you use your writing pins as columns to make the mapping matrix intuitive.  But, you may use them as rows.</param>
	/// <param name="mapping">A mapping between the index of the reading pin in its array and the writing pin in its array to their corresponding characters.
	/// <returns></returns>
	public static MatrixKeypad OpenMatrixKeypad
	(
		this IGpio gpio,
		PinNumber[] readingPins,
		PinNumber[] writingPins,
		char[,] mapping
	)
	{
		if (mapping.GetLength(0) != readingPins.Length)
			throw new Exception("Mapping has an incorrect number of reading pins.");
		if (mapping.GetLength(1) != writingPins.Length)
			throw new Exception("Mapping has an incorrect number of writing pins.");

		var pinReaders = readingPins
			.Select(number => gpio.OpenRead(number, PinReadModes.InputPullDown))
			.ToArray();
		var pinWriters = writingPins
			.Select(number => gpio.OpenWrite(number))
			.ToArray();
		return new(pinReaders, pinWriters, mapping);
	}

	public static async Task<string> ReadUntilCancelAsync(this IKeypad keypad, CancellationToken ct, Action<char>? onKeyPressed = null)
	{
		var list = new List<char>(10);

		void handleKeyPressed(char ch)
		{
			list.Add(ch);

			onKeyPressed?.Invoke(ch);
		}

		try
		{
			keypad.OnKeyPressed += handleKeyPressed;
			await ct.WaitForCancelAsync(CancellationToken.None);
			return new([.. list]);
		}
		finally
		{
			keypad.OnKeyPressed -= handleKeyPressed;
		}
	}
}
