namespace TylerDm.RpGpio;

public static class PinWriterExt
{
	public static PulsingScope PulsingScope(this IPinWriter writer, TimeSpan? onTime = null, TimeSpan? offTime = null) =>
		new(writer, onTime, offTime);

	public static void Pulse(this IPinWriter writer, bool value = true)
	{
		writer.Write(value);
		writer.Write(!value);
	}

	public static async Task PulseAsync(this IPinWriter writer, TimeSpan duration, bool value = true)
	{
		writer.Write(value);
		await Task.Delay(duration);
		writer.Write(!value);
	}

	public static void Toggle(this IPinWriter writer, Action action, bool value = true)
	{
		try
		{
			writer.Write(value);
			action();
		}
		finally
		{
			writer.Write(!value);
		}
	}

	public static async Task ToggleAsync(this IPinWriter writer, Func<Task> func, bool value = true)
	{
		try
		{
			writer.Write(value);
			await func();
		}
		finally
		{
			writer.Write(!value);
		}
	}

	public static async Task<T> ToggleAsync<T>(this IPinWriter writer, Func<Task<T>> func, bool value = true)
	{
		try
		{
			writer.Write(value);
			return await func();
		}
		finally
		{
			writer.Write(!value);
		}
	}
}
