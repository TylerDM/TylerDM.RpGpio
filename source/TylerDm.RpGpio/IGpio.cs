namespace TylerDm.RpGpio;

public interface IGpio
{
	IPinWriter OpenWrite(PinNumber pinNumber);
	IPinReader OpenRead(PinNumber pinNumber);
}
