namespace TylerDm.RpGpio.Devices.Buttons;

public static class ButtonExtensions
{
	public static Button OpenButton(this IGpio gpio, PinNumber pinNo, PinReadModes mode = PinReadModes.InputPullDown) =>
		new(gpio.OpenRead(pinNo, mode));

	public static ButtonGate CreateGate(this Button button)
	{
		button._disposed.ThrowIf();

		return new(button);
	}
}
