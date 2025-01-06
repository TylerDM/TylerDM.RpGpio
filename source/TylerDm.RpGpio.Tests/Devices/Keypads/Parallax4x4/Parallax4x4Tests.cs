namespace TylerDm.RpGpio.Devices.Keypads.Parallax4x4;

//Sorry, not dealing with this right now.
//public class Parallax4x4Tests
//{
//	[Fact]
//	public Task ReadUntilCancelAsync() =>
//		testInContextAsync(async (keypad, p0, p1, p2, p3, p4, p5, p6, p7) =>
//		{
//			using var cts = new CancellationTokenSource();
//			var waitTask = keypad.ReadUntilCancelAsync(cts.Token);
//			Assert.False(waitTask.IsCompleted);

//			//1
//			p7.Pulse();
//			p3.Pulse();
//			await wait1ms();
//			Assert.False(waitTask.IsCompleted);

//			//A
//			p7.Pulse();
//			p0.Pulse();
//			await wait1ms();
//			Assert.False(waitTask.IsCompleted);

//			cts.Cancel();
//			await wait1ms();
//			Assert.True(waitTask.IsCompleted);
//			Assert.True(waitTask.IsCompletedSuccessfully);

//			var text = await waitTask;
//			Assert.Equal("1A", text);
//		});

//	[Fact]
//	public Task Test() =>
//		testInContextAsync(async (keypad, p0, p1, p2, p3, p4, p5, p6, p7) =>
//		{
//			var lastKeyPressed = default(char);
//			keypad.OnKeyPressed += keyPressed => lastKeyPressed = keyPressed;

//			void assert(TestingPin x, TestingPin y, char expected)
//			{
//				x.Pulse();
//				y.Pulse();
//				Assert.Equal(expected, lastKeyPressed);
//				lastKeyPressed = default;
//			}

//			//Test presses
//			assert(p7, p3, '1');
//			assert(p7, p2, '2');
//			assert(p7, p1, '3');
//			assert(p7, p0, 'A');

//			assert(p6, p3, '4');
//			assert(p6, p2, '5');
//			assert(p6, p1, '6');
//			assert(p6, p0, 'B');

//			assert(p5, p3, '7');
//			assert(p5, p2, '8');
//			assert(p5, p1, '9');
//			assert(p5, p0, 'C');

//			assert(p4, p3, '*');
//			assert(p4, p2, '0');
//			assert(p4, p1, '#');
//			assert(p4, p0, 'D');

//			//Recieving 2 signals far apart is noise.
//			lastKeyPressed = default;
//			p7.Pulse();
//			await wait1ms();
//			p3.Pulse();
//			Assert.Equal(default, lastKeyPressed);

//			//Receiving two signals from the same key is noise.
//			await wait1ms();
//			p3.Pulse();
//			p3.Pulse();
//			Assert.Equal(default, lastKeyPressed);

//			//Receivng two signals from different keys, but both are either columns or rows, is noise.
//			await wait1ms();
//			p3.Pulse();
//			p2.Pulse();
//			Assert.Equal(default, lastKeyPressed);
//		});

//	private static Task wait1ms() =>
//		TimeSpan.FromMilliseconds(2).WaitAsync();

//	private static async Task testInContextAsync(Func<Parallax4x4, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, TestingPin, Task> test)
//	{
//		using var gpio = new TestingGpio();
//		using var p0 = gpio.OpenRead(_4, default);
//		using var p1 = gpio.OpenRead(_5, default);
//		using var p2 = gpio.OpenRead(_6, default);
//		using var p3 = gpio.OpenRead(_12, default);
//		using var p4 = gpio.OpenRead(_13, default);
//		using var p5 = gpio.OpenRead(_16, default);
//		using var p6 = gpio.OpenRead(_17, default);
//		using var p7 = gpio.OpenRead(_22, default);
//		using var keypad = gpio.OpenParallax4x4(p0, p1, p2, p3, p4, p5, p6, p7);

//		await test(keypad, p0, p1, p2, p3, p4, p5, p6, p7);
//	}
//}
