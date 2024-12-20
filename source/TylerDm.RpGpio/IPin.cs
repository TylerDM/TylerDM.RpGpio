namespace TylerDm.RpGpio;

public interface IPin : IDisposable
{
	public PinNumber Number { get; }
}
