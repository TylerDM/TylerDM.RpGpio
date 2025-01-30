namespace TylerDm.RpGpio.Devices.Keypads;

public static class KeypadExtensions
{
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
