namespace TylerDm.RpGpio;

public interface IPinReader : IPin
{
	event PinChangedEvent ValueChanged;

	PinReadModes Mode { get; }

	bool Read();
}
