namespace TylerDm.RpGpio.Devices.Keypads.Parallax4x4;

public static class Parallax4x4Extensions
{
	private const PinReadModes _readMode = PinReadModes.InputPullDown;

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
}
