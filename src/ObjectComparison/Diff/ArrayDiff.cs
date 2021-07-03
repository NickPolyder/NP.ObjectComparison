using System;
using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Diff
{
	public class ArrayDiff<TInstance, TArray, TArrayOf> : IDiffInfo<TInstance>
	{
		private readonly ArrayObjectInfo<TInstance, TArray, TArrayOf> _objectInfo;

		public ArrayDiff(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance)
		{
			var originalArray = _objectInfo.GetArray(originalInstance);
			var newArray =  _objectInfo.GetArray(targetInstance);
			
			var hasDeletedItems = originalArray.Length > newArray.Length;
			var hasAddedItems = originalArray.Length < newArray.Length;
			
			var diffItems = new List<ObjectItem>();

			for (int index = 0; index < originalArray.Length && index < newArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var newItem = newArray[index];
				var diffInfoBuilder = new ObjectItem.Builder()
					.SetName($"{_objectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(newItem);

				if (!_objectInfo.IsItemEqual(originalItem, newItem))
				{
					diffInfoBuilder.HasChanges();
				}
				
				diffItems.Add(diffInfoBuilder.Build());
			}

			if (hasDeletedItems)
			{
				for (int index = newArray.Length; index < originalArray.Length; index++)
				{
					var originalItem = originalArray[index];
					var diffInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{index}]")
						.SetOriginalValue(originalItem)
						.SetNewValue(null);
					
					diffInfoBuilder.HasChanges();

					diffItems.Add(diffInfoBuilder.Build());
				}
			}

			if (hasAddedItems)
			{
				for (int index = originalArray.Length; index < newArray.Length; index++)
				{
					var newItem = newArray[index];
					var diffInfoBuilder = new ObjectItem.Builder()
						.SetName($"{_objectInfo.GetName()}[{index}]")
						.SetOriginalValue(null)
						.SetNewValue(newItem);

					diffInfoBuilder.HasChanges();
					diffItems.Add(diffInfoBuilder.Build());
				}
			}
			
			return diffItems;
		}
	}
}