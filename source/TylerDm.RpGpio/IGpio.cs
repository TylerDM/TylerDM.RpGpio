namespace TylerDm.RpGpio;

public interface IGpio : IDisposable
{
	IPinWriter OpenWrite(PinNumber pinNumber);
	IPinReader OpenRead(PinNumber pinNumber, PinReadModes mode);
}
