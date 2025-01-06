namespace TylerDm.RpGpio.Devices.Keypads;

public class KeypadMapping(int one, int two, char c)
{
	public int One { get; } = one;
	public int Two { get; } = two;
	public char Char { get; } = c;
}
