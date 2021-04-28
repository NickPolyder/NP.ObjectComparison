using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPatcher.Results;

namespace ObjectPatcher.Patch
{
	public class ArrayPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public ArrayPatch(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{

			var originalValue = _objectInfo.Get(originalInstance);
			var originalArray = ((IEnumerable<TObject>)originalValue).ToArray();
			var newValue = _objectInfo.Get(targetInstance);
			var newArray = ((IEnumerable<TObject>)newValue).ToArray();

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

				if (!_objectInfo.IsEqual(originalItem, newItem))
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
				_objectInfo.Set(originalInstance, newValue);
			}

			return patchedItems;
		}
	}
}