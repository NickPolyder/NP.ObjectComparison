using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	[DebuggerDisplay("Dictionary Patch: {ObjectInfo.GetName()}")]
	public class DictionaryPatch<TInstance, TKey, TValue> : BaseDictionaryPatch<TInstance, TKey, TValue>
	{
		public DictionaryPatch(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo) : base(objectInfo)
		{  }
		
		protected override IEnumerable<ObjectItem> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
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
				var patchInfoBuilder = new ObjectItem.Builder()
					.SetName($"{ObjectInfo.GetName()}[{modifiedKey}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(targetItem);

				patchInfoBuilder.HasChanges();
				yield return patchInfoBuilder.Build();
			}
		}
	}
}