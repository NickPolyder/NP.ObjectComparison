using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	/// <summary>
	///  Analyzes dictionaries.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[DebuggerDisplay("Dictionary Analyzer: {ObjectInfo.GetName()}")]
	public class DictionaryAnalyzer<TInstance, TKey, TValue> : BaseDictionaryAnalyzer<TInstance, TKey, TValue>
	{
		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <exception cref="System.ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		public DictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo) : base(objectInfo)
		{  }

		/// <inheritdoc />
		protected override IEnumerable<DiffSnapshot> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
		{
			var modifiedKeys = original.Keys
				.Where(originalKey => target.Keys.Contains(originalKey)
				                      && !ObjectInfo.IsValueEquals(original[originalKey], target[originalKey]))
				.ToArray();

			if (modifiedKeys.Length == 0)
			{
				yield break;
			}

			foreach (var modifiedKey in modifiedKeys)
			{
				var originalItem = original[modifiedKey];
				var targetItem = target[modifiedKey];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{modifiedKey}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(targetItem);

				infoBuilder.HasChanges();
				yield return infoBuilder.Build();
			}
		}
	}
}