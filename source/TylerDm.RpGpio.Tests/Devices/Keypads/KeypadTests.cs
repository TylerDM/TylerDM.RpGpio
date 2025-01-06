namespace TylerDm.RpGpio.Devices.Keypads;

public class KeypadTests
{
	private const PinReadModes _pinReadMode = KeypadExtensions._pinReadMode;

	[Fact]
	public Task ReadUntilAsync() =>
		testInContextAsync(async (keypad, p0, p1, p2, p3, p4, p5, p6, p7) =>
		{
			var waitTask = keypad.ReadUntilAsync(2);
			await wait1ms();
			Assert.False(waitTask.IsCompleted);

			//1
			p7.Pulse();
			p3.Pulse();
			await wait1ms();
			Assert.False(waitTask.IsCompleted);

			//A
			p7.Pulse();
			p0.Pulse();
			await wait1ms();
			Assert.True(waitTask.IsCompleted);

			var text = await waitTask;
			Assert.Equal("1A", text);
		});

	[Fact]
	public Task ReadUntilCharAsync() =>
		testInContextAsync(async (keypad, p0, p1, p2, p3, p4, p5, p6, p7) =>
		{
			var waitTask = keypad.ReadUntilAsync(10, '#');
			await wait1ms();
			Assert.False(waitTask.IsCompleted);

			//1
			p7.Pulse();
			p3.Pulse();
			await wait1ms();
			Assert.False(waitTask.IsCompleted);

			//#
			p4.Pulse();
			p1.Pulse();
			await wait1ms();
			Assert.True(waitTask.IsCompleted);

			var text = await waitTask;
			Assert.Equal("1", text);
		});

	[Fact]
	public Task Test() =>
		testInContextAsync(async (keypad, p0, p1, p2, p3, p4, p5, p6, p7) =>
		{
			var lastKeyPressed = default(char);
			keypad.OnKeyPressed += keyPressed => lastKeyPressed = keyPressed;

			void assert(TestingPin x, TestingPin y, char expected)
			{
				x.Pulse();
				y.Pulse();
				Assert.Equal(expected, lastKeyPressed);
				lastKeyPressed = default;
			}

			//Test presses
			assert(p7, p3, '1');
			assert(p7, p2, '2');
			assert(p7, p1, '3');
			assert(p7, p0, 'A');

			assert(p6, p3, '4');
			assert(p6, p2, '5');
			assert(p6, p1, '6');
			assert(p6, p0, 'B');

			assert(p5, p3, '7');
			assert(p5, p2, '8');
			assert(p5, p1, '9');
			assert(p5, p0, 'C');

			assert(p4, p3, '*');
			assert(p4, p2, '0');
			assert(p4, p1, '#');
			assert(p4, p0, 'D');

			//Recieving 2 signals far apart is noise.
			lastKeyPressed = default;
			p7.Pulse();
			await wait1ms();
			p3.Pulse();
			Assert.Equal(default, lastKeyPressed);

			//Receiving two signals from the same key is noise.
			await wait1ms();
			p3.Pulse();
			p3.Pulse();
			Assert.Equal(default, lastKeyPressed);

			//Receivng two signals from different keys, but both are either columns or rows, is noise.
			await wait1ms();
			p3.Pulse();
			p2.Pulse();
			Assert.Equal(default, lastKeyPressed);
		});

	private static Task wait1ms() =>
		TimeSpan.FromMilliseconds(2).WaitAsync();

	private static async Task testInContextAsync(Func<Keypad, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, Task> test)
	{
		using var gpio = new TestingGpio();
		using var p0 = gpio.OpenRead(_4, _pinReadMode);
		using var p1 = gpio.OpenRead(_5, _pinReadMode);
		using var p2 = gpio.OpenRead(_6, _pinReadMode);
		using var p3 = gpio.OpenRead(_12, _pinReadMode);
		using var p4 = gpio.OpenRead(_13, _pinReadMode);
		using var p5 = gpio.OpenRead(_16, _pinReadMode);
		using var p6 = gpio.OpenRead(_17, _pinReadMode);
		using var p7 = gpio.OpenRead(_22, _pinReadMode);
		using var keypad = gpio.OpenParallax4x4(p0, p1, p2, p3, p4, p5, p6, p7);

		await test(keypad, p0, p1, p2, p3, p4, p5, p6, p7);
	}
}
