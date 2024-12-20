namespace TylerDm.RpGpio;

public interface IPinReader : IPin
{
	event PinChangedEvent ValueChanged;

	bool Read();
}
