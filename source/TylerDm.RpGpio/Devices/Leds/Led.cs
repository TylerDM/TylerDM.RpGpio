﻿namespace TylerDm.RpGpio.Devices.Leds;

public class Led : WritingDevice
{
	//CTS from _disposed not to be used here as we need a resettable one.
	private readonly DisposedTracker<Led> _disposed = new();

	private Cts cts = new();

	public bool Lit
	{
		get => getValue();
		set
		{
			methodPreflight();

			setValue(value);
		}
	}

	public Led(IPinWriter pin, bool? defaultState = false) : base(pin)
	{
		if (defaultState is bool ds)
			setValue(ds);
	}

	public void Toggle()
	{
		methodPreflight();

		Lit = !Lit;
	}

	public async Task FlashAsync(TimeSpan duration = default, Ct ct = default)
	{
		methodPreflight();

		if (duration == default) duration = FlashSettings.DefaultDuration;

		using var linkedCts = cts.CreateLinked(ct);

		setValue(true);
		await duration.TryWaitAsync(linkedCts);
		setValue(false);
	}

	public async Task FlashAsync(int count, FlashSettings? settings = default, Ct ct = default)
	{
		methodPreflight();

		settings ??= FlashSettings.Default;

		using var linkedCts = cts.CreateLinked(ct);

		for (var i = 0; i < count; i++)
		{
			await FlashAsync(settings.OnDuration, ct);

			//Won't see the benefit of FlashAsync's internal token linking.
			//So we need to handle it ourselves here.
			await settings.OffDuration.TryWaitAsync(linkedCts);
		}
	}

	public async Task FlashUntilAsync(Ct ct, FlashSettings? settings = default)
	{
		methodPreflight();

		settings ??= FlashSettings.Default;

		using var linkedCts = cts.CreateLinked(ct);
		while (ct.ShouldContinue())
		{
			setValue(true);
			await settings.OnDuration.TryWaitAsync(ct);
			setValue(false);
			await settings.OffDuration.TryWaitAsync(ct);
		}
	}

	public override void Dispose()
	{
		if (_disposed) return;
		_disposed.Dispose();

		cts.Dispose();
		base.Dispose();
	}

	private void methodPreflight()
	{
		_disposed.ThrowIf();
		cancelPreviousOperations();
	}

	private void cancelPreviousOperations()
	{
		cts.Cancel();
		cts = new();
	}
}
