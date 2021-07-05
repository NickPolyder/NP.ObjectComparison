using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	[DebuggerDisplay("Array Patch: {ObjectInfo.GetName()}")]
	public class ArrayPatch<TInstance, TArray, TArrayOf> : BaseArrayPatch<TInstance, TArray, TArrayOf>
	{
		public ArrayPatch(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo) : base(objectInfo)
		{
		}

		protected override IEnumerable<ObjectItem> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			for (int index = 0; index < originalArray.Length && index < targetArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var newItem = targetArray[index];
				var patchInfoBuilder = new ObjectItem.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(newItem);

				if (!ObjectInfo.IsItemEqual(originalItem, newItem))
				{
					patchInfoBuilder.HasChanges();
				}

				yield return patchInfoBuilder.Build();
			}
		}
	}
}