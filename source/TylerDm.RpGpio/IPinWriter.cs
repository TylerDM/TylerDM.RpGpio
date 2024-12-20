namespace TylerDm.RpGpio;

public interface IPinWriter : IPin
{
	void Write(bool value);

	public PulsingScope PulsingScope(TimeSpan? onTime = null, TimeSpan? offTime = null) =>
		new(this, onTime, offTime);

	public void Pulse(bool value = true)
	{
		Write(value);
		Write(!value);
	}

	public async Task PulseAsync(TimeSpan duration, bool value = true)
	{
		Write(value);
		await Task.Delay(duration);
		Write(!value);
	}

	public void Toggle(Action action, bool value = true)
	{
		try
		{
			Write(value);
			action();
		}
		finally
		{
			Write(!value);
		}
	}

	public async Task ToggleAsync(Func<Task> func, bool value = true)
	{
		try
		{
			Write(value);
			await func();
		}
		finally
		{
			Write(!value);
		}
	}

	public async Task<T> ToggleAsync<T>(Func<Task<T>> func, bool value = true)
	{
		try
		{
			Write(value);
			return await func();
		}
		finally
		{
			Write(!value);
		}
	}
}
