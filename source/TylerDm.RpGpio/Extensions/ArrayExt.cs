namespace TylerDm.RpGpio.Extensions;

public static class ArrayExt
{
	internal static int? IndexOf<T>(this T[] array, T element)
	{
		var index = Array.IndexOf(array, element);
		if (index == -1) return null;
		return index;
	}

	internal static int IndexOfRequired<T>(this T[] array, T element) =>
		array.IndexOf(element) ?? throw new Exception("Array does not contain element.");
}
