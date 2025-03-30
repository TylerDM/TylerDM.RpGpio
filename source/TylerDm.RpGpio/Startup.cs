namespace TylerDm.RpGpio;

public static class Startup
{
	public static void AddRpGpio(this IServiceCollection services, Func<Gpio>? createGpio = null)
	{
		services.AddSingleton<Gpio>(createGpio?.Invoke() ?? new());
		services.AddSingleton<IGpio, Gpio>(x => x.GetRequiredService<Gpio>());
	}
}
