namespace TylerDm.RpGpio;

public static class Startup
{
	public static void AddRpGpio(this IServiceCollection services) =>
		services.AddOrangePeeledServices();
}
