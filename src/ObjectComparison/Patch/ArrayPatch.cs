using System;
using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	public class ArrayPatch<TInstance, TArray, TArrayOf> : IPatchInfo<TInstance>
	{
		private readonly ArrayObjectInfo<TInstance, TArray, TArrayOf> _objectInfo;

		public ArrayPatch(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			var originalArray = _objectInfo.GetArray(originalInstance);
			var newArray =  _objectInfo.GetArray(targetInstance);
			
			var hasDeletedItems = originalArray.Length > newArray.Length;
			var hasAddedItems = originalArray.Length < newArray.Length;

			bool hasChanges = hasDeletedItems || hasAddedItems;

			var patchedItems = new List<ObjectItem>();

			for (int index = 0; index < originalArray.Length && index < newArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var newItem = newArray[index];
				var patchInfoBuilder = new ObjectItem.Builder()
					.SetName($"{_objectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(newItem);

				if (!_objectInfo.IsItemEqual(originalItem, newItem))
				{
					patchInfoBuilder.HasChanges();
					hasChanges = true;
				}
				patchedItems.Add(patchInfoBuilder.Build());
			}

			if (hasDeletedItems)
			{
				for (int index = newArray.Length; index < originalArray.Length; index++)
				{
					var originalItem = originalArray[index];
					var patchInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{index}]")
						.SetOriginalValue(originalItem)
						.SetNewValue(null);
					
					patchInfoBuilder.HasChanges();
					patchedItems.Add(patchInfoBuilder.Build());
				}
			}

			if (hasAddedItems)
			{
				for (int index = originalArray.Length; index < newArray.Length; index++)
				{
					var newItem = newArray[index];
					var patchInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{index}]")
						.SetOriginalValue(null)
						.SetNewValue(newItem);

					patchInfoBuilder.HasChanges();
					patchedItems.Add(patchInfoBuilder.Build());
				}
			}

			if (hasChanges)
			{
				_objectInfo.Set(originalInstance, _objectInfo.Get(targetInstance));
			}

			return patchedItems;
		}
	}
}