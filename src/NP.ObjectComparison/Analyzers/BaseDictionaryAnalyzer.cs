using System;
using System.Collections.Generic;
using System.Linq;
using NP.ObjectComparison.Analyzers.Infos;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison.Analyzers
{
	/// <summary>
	/// Abstract functionality of dictionary analyzers.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public abstract class BaseDictionaryAnalyzer<TInstance, TKey, TValue> : IObjectAnalyzer<TInstance>
	{
		/// <summary>
		/// The object info for this Dictionary.
		/// </summary>
		protected readonly DictionaryObjectInfo<TInstance, TKey, TValue> ObjectInfo;

		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <exception cref="ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		protected BaseDictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo)
		{
			ObjectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		/// <inheritdoc />
		public IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			var diffAnalysisResult = new DiffAnalysisResult();
			if (originalInstance == null || targetInstance == null)
			{
				return diffAnalysisResult;
			}
			
			var original = ObjectInfo.Get(originalInstance);
			var target = ObjectInfo.Get(targetInstance);

			var deletedItems = HandleDeletedItems(original, target).ToArray();
			
			var addedItems = HandleAddedItems(original, target).ToArray();
			
			var modifiedItems = HandleModifiedItems(original, target).ToArray();
			
			var hasChanges = deletedItems.Length > 0
			                 || addedItems.Length > 0
			                 || modifiedItems.Any(item => item.HasChanges);

			if (hasChanges)
			{
				diffAnalysisResult.AddPatchAction(() => ObjectInfo.Set(originalInstance, target));
			}

			diffAnalysisResult.AddRange(deletedItems);
			diffAnalysisResult.AddRange(addedItems);
			diffAnalysisResult.AddRange(modifiedItems);

			return diffAnalysisResult;
		}

		/// <summary>
		/// Handles deleted items.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="target"></param>
		/// <returns>The list of <see cref="DiffSnapshot"/> related to the deleted items.</returns>
		protected virtual IEnumerable<DiffSnapshot> HandleDeletedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
		{
			var deletedKeys = original.Keys
				.Where(originalKey =>
					!target.Keys.Any(targetKey => ObjectInfo.IsKeyEquals(originalKey, targetKey)))
				.ToArray();
			if (deletedKeys.Length == 0)
			{
				yield break;
			}

			foreach (var deletedKey in deletedKeys)
			{
				var originalItem = original[deletedKey];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{deletedKey}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(null)
					.HasChanges();

				yield return infoBuilder.Build();
			}
		}

		/// <summary>
		/// Handles added items.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="target"></param>
		/// <returns>The list of <see cref="DiffSnapshot"/> related to the added items.</returns>
		protected virtual IEnumerable<DiffSnapshot> HandleAddedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
		{
			var addedKeys = target.Keys
				.Where(targetKey =>
					!original.Keys.Any(originalKey => ObjectInfo.IsKeyEquals(originalKey, targetKey)))
				.ToArray();
			
			if (addedKeys.Length == 0)
			{
				yield break;
			}

			foreach (var addedKey in addedKeys)
			{
				var targetItem = target[addedKey];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{addedKey}]")
					.SetOriginalValue(null)
					.SetNewValue(targetItem)
					.HasChanges();

				yield return infoBuilder.Build();
			}
		}

		/// <summary>
		/// Handles modified items.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="target"></param>
		/// <returns>The list of <see cref="DiffSnapshot"/> related to the modified items.</returns>
		protected abstract IEnumerable<DiffSnapshot> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target);
	}
}