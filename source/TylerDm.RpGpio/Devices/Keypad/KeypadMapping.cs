namespace TylerDm.RpGpio.Devices.Keypad;

public class KeypadMapping(int one, int two, char c)
{
	public int One { get; } = one;
	public int Two { get; } = two;
	public char Char { get; } = c;
}
