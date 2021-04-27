using System;
using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher
{
	public class PropertyPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public PropertyPatch(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}
		
		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			yield return _objectInfo.Patch(originalInstance, targetInstance);
		}
	}
}