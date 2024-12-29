namespace TylerDm.RpGpio;

public class PinReader : Pin, IPinReader
{
	private PinChangedEvent? valueChanged;

	public PinReadModes Mode { get; }

	public event PinChangedEvent ValueChanged
	{
		add => valueChanged += value;
		remove => valueChanged -= value;
	}

	public PinReader(GpioController controller, GpioPin pin) : base(controller, pin)
	{
		Mode = pin.GetPinMode().ToEnum<PinReadModes>();
		_pin.ValueChanged += handleValueChanged;
	}

	public override void Dispose()
	{
		base.Dispose();
		_pin.ValueChanged -= handleValueChanged;
	}

	public bool Read() =>
		(bool)_pin.Read();

	private void handleValueChanged(object sender, PinValueChangedEventArgs args) =>
		valueChanged?.Invoke((PinEventTypes)args.ChangeType);
}
