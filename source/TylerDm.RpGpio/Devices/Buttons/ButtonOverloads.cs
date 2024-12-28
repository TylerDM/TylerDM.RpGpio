namespace TylerDm.RpGpio.Devices.Buttons;

public static class ButtonOverloads
{
	public static async Task<bool> TryWaitForPressesAsync(this Button button, int count, TimeSpan timeout = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		using var timedCts = new Cts(timeout);
		return await button.TryWaitForPressesAsync(count, timedCts.Token);
	}

	public static async Task<bool> TryWaitForPressesAsync(this Button button, int count, Ct ct)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		for (var i = 0; i < count; i++)
		{
			var result = await button.TryWaitForPressAsync(ct);
			if (result == false) return false;
		}
		return true;
	}

	public static async Task<bool> TryWaitForPressAsync(this Button button, TimeSpan timeout = default)
	{
		using var timedCts = new Cts(timeout);
		return await button.TryWaitForPressAsync(timedCts.Token);
	}
}
