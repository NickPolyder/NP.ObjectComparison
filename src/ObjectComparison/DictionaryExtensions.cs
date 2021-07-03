using System;
using System.Collections.Generic;

namespace ObjectComparison
{
	public static class DictionaryExtensions
	{
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
			Func<TKey, TValue> valueFunc)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}

			if (valueFunc == null)
			{
				throw new ArgumentNullException(nameof(valueFunc));
			}

			if (dictionary.TryGetValue(key, out var result))
			{
				return result;
			}

			var generatedValue = valueFunc(key);
			dictionary.Add(key, generatedValue);

			return generatedValue;
		}
	}
}