using System;
using System.Collections.Generic;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	public abstract class BaseArrayAnalyzer<TInstance, TArray, TArrayOf> : IObjectAnalyzer<TInstance>
	{
		protected readonly ArrayObjectInfo<TInstance, TArray, TArrayOf> ObjectInfo;

		protected BaseArrayAnalyzer(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo)
		{
			ObjectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public virtual IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			var diffAnalysisResult = new DiffAnalysisResult();
			if (originalInstance == null || targetInstance == null)
			{
				return diffAnalysisResult;
			}

			var originalArray = ObjectInfo.GetArray(originalInstance);
			var targetArray = ObjectInfo.GetArray(targetInstance);

			var deletedItems = HandleDeletedItems(originalArray, targetArray).ToArray();
			
			var addedItems = HandleAddedItems(originalArray, targetArray).ToArray();

			var modifiedItems = HandleModifiedItems(originalArray, targetArray).ToArray();
			
			var hasChanges = deletedItems.Length > 0
			                 || addedItems.Length > 0
			                 || modifiedItems.Any(item => item.HasChanges);

			if (hasChanges)
			{
				diffAnalysisResult.AddPatchAction(() => ObjectInfo.Set(originalInstance, ObjectInfo.Get(targetInstance)));
			}

			diffAnalysisResult.AddRange(deletedItems);
			diffAnalysisResult.AddRange(addedItems);
			diffAnalysisResult.AddRange(modifiedItems);

			return diffAnalysisResult;
		}

		protected virtual IEnumerable<DiffSnapshot> HandleDeletedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			if (originalArray.Length <= targetArray.Length)
			{
				yield break;
			}

			for (int index = targetArray.Length; index < originalArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(null)
					.HasChanges();

				yield return infoBuilder.Build();
			}
		}

		protected virtual IEnumerable<DiffSnapshot> HandleAddedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			if (originalArray.Length >= targetArray.Length)
			{
				yield break;
			}

			for (int index = originalArray.Length; index < targetArray.Length; index++)
			{
				var newItem = targetArray[index];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(null)
					.SetNewValue(newItem)
					.HasChanges();

				yield return infoBuilder.Build();
			}
		}

		protected abstract IEnumerable<DiffSnapshot> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray);
	}
}