using System;
using System.Collections.Generic;
using System.Linq;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	public abstract class BaseArrayPatch<TInstance, TArray, TArrayOf> : IPatchInfo<TInstance>
	{
		protected readonly ArrayObjectInfo<TInstance, TArray, TArrayOf> ObjectInfo;

		protected BaseArrayPatch(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo)
		{
			ObjectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public virtual IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
			{
				yield break;
			}

			if (targetInstance == null)
			{
				yield break;
			}

			var originalArray = ObjectInfo.GetArray(originalInstance);
			var targetArray = ObjectInfo.GetArray(targetInstance);

			var deletedItems = HandleDeletedItems(originalArray, targetArray).ToArray();
			
			foreach (var item in deletedItems)
			{
				yield return item;
			}

			var addedItems = HandleAddedItems(originalArray, targetArray).ToArray();

			foreach (var item in addedItems)
			{
				yield return item;
			}

			var modifiedItems = HandleModifiedItems(originalArray, targetArray).ToArray();

			foreach (var item in modifiedItems)
			{
				yield return item;
			}

			var hasChanges = deletedItems.Length > 0
			                 || addedItems.Length > 0
			                 || modifiedItems.Any(item => item.HasChanges);

			if (hasChanges)
			{
				ObjectInfo.Set(originalInstance, ObjectInfo.Get(targetInstance));
			}
		}

		protected virtual IEnumerable<ObjectItem> HandleDeletedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			if (originalArray.Length <= targetArray.Length)
			{
				yield break;
			}

			for (int index = targetArray.Length; index < originalArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var patchInfoBuilder = new ObjectItem.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(null);

				patchInfoBuilder.HasChanges();
				yield return patchInfoBuilder.Build();
			}
		}

		protected virtual IEnumerable<ObjectItem> HandleAddedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			if (originalArray.Length >= targetArray.Length)
			{
				yield break;
			}

			for (int index = originalArray.Length; index < targetArray.Length; index++)
			{
				var newItem = targetArray[index];
				var patchInfoBuilder = new ObjectItem.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(null)
					.SetNewValue(newItem);

				patchInfoBuilder.HasChanges();
				yield return patchInfoBuilder.Build();
			}
		}

		protected abstract IEnumerable<ObjectItem> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray);
	}
}