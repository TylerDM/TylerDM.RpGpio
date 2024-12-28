namespace TylerDm.RpGpio.Devices.Leds;

public class FlashSettings
{
	public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(1);
	public static readonly FlashSettings Default = new();

	public TimeSpan OnDuration { get; init; } = DefaultDuration;
	public TimeSpan OffDuration { get; init; } = DefaultDuration;
}
