using System;
using System.Collections.Generic;
using System.Linq;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	public class DictionaryPatch<TInstance, TKey, TValue> : IPatchInfo<TInstance>
	{
		private readonly DictionaryObjectInfo<TInstance, TKey, TValue> _objectInfo;

		public DictionaryPatch(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
			{
				return Enumerable.Empty<ObjectItem>();
			}

			if (targetInstance == null)
			{
				return Enumerable.Empty<ObjectItem>();
			}

			var original = _objectInfo.Get(originalInstance);
			var target = _objectInfo.Get(targetInstance);
			var originalKeys = original.Keys;
			var targetKeys = target.Keys;

			var deletedKeys = originalKeys
				.Where(originalKey => 
					!targetKeys.Any(targetKey => _objectInfo.IsKeyEquals(originalKey, targetKey)))
				.ToArray();

			var addedKeys = targetKeys
				.Where(targetKey =>
					!originalKeys.Any(originalKey => _objectInfo.IsKeyEquals(originalKey, targetKey)))
				.ToArray();

			var modifiedKeys = originalKeys
				.Where(originalKey => targetKeys.Contains(originalKey)
					&& !_objectInfo.IsValueEquals(original[originalKey], target[originalKey]))
				.ToArray();

			var hasDeletedItems = deletedKeys.Length > 0;
			var hasAddedItems = addedKeys.Length > 0;
			var hasModifiedItems = modifiedKeys.Length > 0;

			bool hasChanges = hasDeletedItems || hasAddedItems || hasModifiedItems;

			var patchedItems = new List<ObjectItem>();

			if (hasDeletedItems)
			{
				foreach (var deletedKey in deletedKeys)
				{
					var originalItem = original[deletedKey];
					var patchInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{deletedKey}]")
						.SetOriginalValue(originalItem)
						.SetNewValue(null);

					patchInfoBuilder.HasChanges();
					patchedItems.Add(patchInfoBuilder.Build());
				}
			}

			if (hasAddedItems)
			{
				foreach (var addedKey in addedKeys)
				{
					var targetItem = target[addedKey];
					var patchInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{addedKey}]")
						.SetOriginalValue(null)
						.SetNewValue(targetItem);

					patchInfoBuilder.HasChanges();
					patchedItems.Add(patchInfoBuilder.Build());
				}
			}

			if (hasModifiedItems)
			{
				foreach (var modifiedKey in modifiedKeys)
				{
					var originalItem = original[modifiedKey];
					var targetItem = target[modifiedKey];
					var patchInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{modifiedKey}]")
						.SetOriginalValue(originalItem)
						.SetNewValue(targetItem);

					patchInfoBuilder.HasChanges();
					patchedItems.Add(patchInfoBuilder.Build());
				}
			}
			
			if (hasChanges)
			{
				_objectInfo.Set(originalInstance, target);
			}

			return patchedItems;
		}
	}
}