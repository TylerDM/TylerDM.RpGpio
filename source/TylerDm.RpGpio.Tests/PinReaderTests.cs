using TylerDm.RpGpio.Extensions;

namespace TylerDm.RpGpio;

public class PinReaderTests
{
	[Fact]
	public async Task WaitForPulsesAsync()
	{
		var gpio = new TestingGpio();
		var pin = gpio.OpenRead(default);
		var writablePin = (IPinWriter)pin;

		var waitTask = Task.Run(() => pin.WaitForPulsesAsync(2, TimeSpan.FromHours(1)));
		await letPropagateAsync();
		Assert.False(waitTask.IsCompletedSuccessfully);

		writablePin.Pulse();
		await letPropagateAsync();
		Assert.False(waitTask.IsCompletedSuccessfully);

		writablePin.Pulse();
		await letPropagateAsync();
		Assert.True(waitTask.IsCompletedSuccessfully);
	}

	[Fact]
	public async Task CountPulsesAsync()
	{
		using var cts = new CancellationTokenSource();
		var gpio = new TestingGpio();

		var read = gpio.OpenRead(default);
		var write = (IPinWriter)read;

		var waitTask = Task.Run(() => read.CountPulsesAsync(cts.Token));
		Assert.False(waitTask.IsCompleted);
		await letPropagateAsync();

		write.Pulse();
		write.Pulse();
		write.Pulse();
		Assert.False(waitTask.IsCompleted);

		cts.Cancel();
		await waitTask.WaitAsync(TimeSpan.FromMilliseconds(100));
		Assert.True(waitTask.IsCompletedSuccessfully);

		var result = await waitTask;
		Assert.Equal(3, result);
	}

	[Fact]
	public async Task WaitForPulsesTimeoutAsync()
	{
		var gpio = new TestingGpio();
		var pin = gpio.OpenRead(default);

		var waitTask = Task.Run(() => pin.WaitForPulsesAsync(2, TimeSpan.FromMilliseconds(10)));
		Assert.False(waitTask.IsCompletedSuccessfully);
		await waitTask.TryWaitAsync(TimeSpan.FromMilliseconds(20));
		Assert.True(waitTask.IsCompleted);
		Assert.False(waitTask.IsCompletedSuccessfully);
	}

	private static Task letPropagateAsync(int ms = 10) =>
		Task.Delay(TimeSpan.FromMilliseconds(ms));
}
