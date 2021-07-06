using System.Collections.Generic;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	public abstract class BaseDictionaryAnalyzer<TInstance, TKey, TValue> : IObjectAnalyzer<TInstance>
	{
		protected readonly DictionaryObjectInfo<TInstance, TKey, TValue> ObjectInfo;

		protected BaseDictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo)
		{
			ObjectInfo = objectInfo;
		}

		public IEnumerable<DiffSnapshot> Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
			{
				yield break;
			}

			if (targetInstance == null)
			{
				yield break;
			}

			var original = ObjectInfo.Get(originalInstance);
			var target = ObjectInfo.Get(targetInstance);

			var deletedItems = HandleDeletedItems(original, target).ToArray();

			foreach (var item in deletedItems)
			{
				yield return item;
			}

			var addedItems = HandleAddedItems(original, target).ToArray();

			foreach (var item in addedItems)
			{
				yield return item;
			}

			var modifiedItems = HandleModifiedItems(original, target).ToArray();

			foreach (var item in modifiedItems)
			{
				yield return item;
			}

			var hasChanges = deletedItems.Length > 0
			                 || addedItems.Length > 0
			                 || modifiedItems.Any(item => item.HasChanges);

			if (hasChanges)
			{
				ObjectInfo.Set(originalInstance, target);
			}
		}

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
					.SetNewValue(null);

				infoBuilder.HasChanges();
				yield return infoBuilder.Build();
			}
		}

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
					.SetNewValue(targetItem);

				infoBuilder.HasChanges();
				yield return infoBuilder.Build();
			}
		}

		protected abstract IEnumerable<DiffSnapshot> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target);
	}
}