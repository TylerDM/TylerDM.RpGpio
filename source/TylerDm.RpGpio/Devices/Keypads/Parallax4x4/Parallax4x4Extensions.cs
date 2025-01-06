namespace TylerDm.RpGpio.Devices.Keypads.Parallax4x4;

public static class Parallax4x4Extensions
{
	private const PinReadModes _readMode = PinReadModes.InputPullDown;

	public static async Task<string> ReadUntilCancelAsync(this Parallax4x4 keypad, CancellationToken ct, Action<char>? onKeyPressed = null)
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

	public static Parallax4x4 OpenParallax4x4(this IGpio gpio,
			PinNumber p0w, PinNumber p1w, PinNumber p2w, PinNumber p3w,
			PinNumber p4r, PinNumber p5r, PinNumber p6r, PinNumber p7r
		) =>
		new
		(
			gpio.OpenWrite(p0w),
			gpio.OpenWrite(p1w),
			gpio.OpenWrite(p2w),
			gpio.OpenWrite(p3w),

			gpio.OpenRead(p4r, _readMode),
			gpio.OpenRead(p5r, _readMode),
			gpio.OpenRead(p6r, _readMode),
			gpio.OpenRead(p7r, _readMode)
		);

	public static Parallax4x4 OpenParallax4x4(this IGpio gpio,
		IPinWriter p0w, IPinWriter p1w, IPinWriter p2w, IPinWriter p3w,
		IPinReader p4r, IPinReader p5r, IPinReader p6r, IPinReader p7r
	) =>
		new
		(
			p0w, p1w, p2w, p3w,
			p4r, p5r, p6r, p7r
		);
}
