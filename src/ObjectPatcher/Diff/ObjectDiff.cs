using System;
using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher.Diff
{
	public class ObjectDiff<TInstance, TObject> : IDiffInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public ObjectDiff(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance)
		{
			var originalValue = _objectInfo.Get(originalInstance);
			var newValue = _objectInfo.Get(targetInstance);
			var patchInfoBuilder = new ObjectItem.Builder()
				.SetName(_objectInfo.GetName())
				.SetOriginalValue(originalValue)
				.SetNewValue(newValue);

			if (!_objectInfo.IsEqual(originalValue, newValue))
			{
				patchInfoBuilder.HasChanges();
			}

			yield return patchInfoBuilder.Build();
		}
	}
}