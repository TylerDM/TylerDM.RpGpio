namespace TylerDm.RpGpio.Devices.Keypads;

public class KeypadMap(params IEnumerable<KeypadMapping> _mappings)
{
	private readonly FrozenDictionary<(int, int), char> _dictionary =
		_mappings.ToFrozenDictionary(
			createKey,
			mapping => mapping.Char
		);

	public char? GetChar(int one, int two)
	{
		var key = createKey(one, two);
		return getValue(key);
	}

	private static (int low, int higher) createKey(KeypadMapping mapping) =>
		createKey(mapping.One, mapping.Two);

	private static (int lower, int higher) createKey(int one, int two)
	{
		var lower = one < two ? one : two;
		var higher = one < two ? two : one;
		return (lower, higher);
	}

	private char? getValue((int lower, int higher) key)
	{
		if (_dictionary.TryGetValue(key, out var c)) return c;
		return null;
	}
}
