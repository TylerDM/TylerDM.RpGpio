namespace TylerDm.RpGpio.Devices.Buttons;

public static class ButtonExtensions
{
	public static Button OpenButton(this IGpio gpio, PinNumber pinNo, PinReadModes mode = PinReadModes.InputPullDown) =>
		new(gpio.OpenRead(pinNo, mode));

	public static async Task WaitForPressesAsync(this Button button, int count, CancellationToken ct = default)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

		var value = 0;
		using var gate = new Gate();
		void handlePressed()
		{
			value++;
			if (value >= count)
				gate.Open();
		}

		button.OnPressedEvent += handlePressed;
		try
		{
			await gate.WaitAsync(ct);
		}
		finally
		{
			button.OnPressedEvent -= handlePressed;
		}
	}

	public static async Task WaitForPressAsync(this Button button, CancellationToken ct = default)
	{
		using var gate = new Gate();
		void handlePressed() => gate.Open();

		button.OnPressedEvent += handlePressed;
		try
		{
			await gate.WaitAsync(ct);
		}
		finally
		{
			button.OnPressedEvent -= handlePressed;
		}
	}
}
