using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	[DebuggerDisplay("Dictionary Patch: {ObjectInfo.GetName()}")]
	public class ComplexDictionaryPatch<TInstance, TKey, TValue> : BaseDictionaryPatch<TInstance, TKey, TValue>
	{
		private readonly IEnumerable<IPatchInfo<TValue>> _patches;
		public ComplexDictionaryPatch(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo, IEnumerable<IPatchInfo<TValue>> patches) : base(objectInfo)
		{
			_patches = patches;
		}

		protected override IEnumerable<ObjectItem> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
		{
			var sameKeys = original.Keys
				.Where(originalKey => target.Keys.Contains(originalKey))
				.ToArray();

			if (sameKeys.Length == 0)
			{
				yield break;
			}

			foreach (var patchInfo in _patches)
			{
				foreach (var key in sameKeys)
				{
					var originalItem = original[key];
					var targetItem = target[key];
					
					foreach (var objectItem in patchInfo.Patch(originalItem, targetItem))
					{
						yield return new ObjectItem.Builder(objectItem)
							.SetPrefix($"{ObjectInfo.GetName()}[{key}]")
							.Build();
					}
				}
			}
		}
	}
}