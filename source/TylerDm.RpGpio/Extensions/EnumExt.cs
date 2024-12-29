namespace TylerDm.RpGpio.Extensions;

public static class EnumExt
{
	public static void RequireDefined<T>(this T value)
		where T : struct, Enum
	{
		if (Enum.IsDefined(value)) return;
		throw new Exception("Enum does not contain this value.");
	}

	public static TTo ToEnum<TTo>(this Enum from)
		where TTo : struct, Enum
	{
		var to = (TTo)(object)from;
		to.RequireDefined();
		return to;
	}
}
