namespace TylerDm.RpGpio;

public interface IPin : IDisposable
{
	PinNumber Number { get; }
}
