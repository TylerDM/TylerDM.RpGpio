namespace TylerDm.RpGpio;

public static class Startup
{
	public static void AddRpGpio(this IServiceCollection services)
	{
		services.AddSingleton<Gpio>();
		services.AddSingleton<IGpio, Gpio>(x => x.GetRequiredService<Gpio>());
	}
}
