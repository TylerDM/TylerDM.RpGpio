namespace TylerDm.RpGpio;

public interface IPinWriter : IPin
{
	void Write(bool value);
}
