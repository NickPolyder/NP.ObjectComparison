using System;
using System.Collections.Generic;

namespace ObjectComparison
{
	/// <summary>
	/// Extensions for <see cref="IDictionary{TKey, TValue}"/>.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Gets a value or adds to the dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary">This dictionary.</param>
		/// <param name="key">The key to fetch.</param>
		/// <param name="valueFunc">Value generator.</param>
		/// <returns>The <typeparamref name="TValue"/> related to the <paramref name="key"/>.</returns>
		/// <exception cref="ArgumentNullException">
		/// When the <paramref name="dictionary"/> or <paramref name="valueFunc"/>
		/// are null.
		/// </exception>
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