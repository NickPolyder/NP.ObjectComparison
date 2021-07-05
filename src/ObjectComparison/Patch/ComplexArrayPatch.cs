using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	[DebuggerDisplay("Complex Array Patch: {ObjectInfo.GetName()}")]
	public class ComplexArrayPatch<TInstance, TArray, TArrayOf> : BaseArrayPatch<TInstance, TArray, TArrayOf>, IPatchInfo<TInstance>
	{
		private readonly IEnumerable<IPatchInfo<TArrayOf>> _patches;
		public ComplexArrayPatch(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo, IEnumerable<IPatchInfo<TArrayOf>> patches) : base(objectInfo)
		{
			_patches = patches;
		}
		
		protected override IEnumerable<ObjectItem> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			foreach (var patchInfo in _patches)
			{
				for (int index = 0; index < originalArray.Length && index < targetArray.Length; index++)
				{
					var originalItem = originalArray[index];
					var targetItem = targetArray[index];
					foreach (var objectItem in patchInfo.Patch(originalItem, targetItem))
					{
						yield return new ObjectItem.Builder(objectItem)
							.SetPrefix($"{ObjectInfo.GetName()}[{index}]")
							.Build();
					}
				}
			}
		}
	}
}