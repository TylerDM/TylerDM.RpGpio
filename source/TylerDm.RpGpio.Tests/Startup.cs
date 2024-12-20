namespace TylerDm.RpGpio;

public class Startup
{
	public void ConfigureHost(IHostBuilder builder)
	{
	}

	public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
	{
		services.AddOrangePeeledServices();
	}

	public void Configure(IHost host)
	{
	}
}
