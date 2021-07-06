using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	[DebuggerDisplay("Dictionary Analyzer: {ObjectInfo.GetName()}")]
	public class DictionaryAnalyzer<TInstance, TKey, TValue> : BaseDictionaryAnalyzer<TInstance, TKey, TValue>
	{
		public DictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo) : base(objectInfo)
		{  }
		
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