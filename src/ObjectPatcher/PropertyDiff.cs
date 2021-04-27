using System;
using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher
{
	public class PropertyDiff<TInstance, TObject> : IDiffInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public PropertyDiff(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance)
		{
			yield return _objectInfo.Diff(originalInstance, targetInstance);
		}
	}
}