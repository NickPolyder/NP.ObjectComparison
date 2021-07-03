using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	[DebuggerDisplay("Object Patch: {_objectInfo.GetName()}")]
	public class ObjectPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public ObjectPatch(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
				yield break;

			if (targetInstance == null)
				yield break;

			var originalValue = _objectInfo.Get(originalInstance);
			var newValue = _objectInfo.Get(targetInstance);
			var patchInfoBuilder = new ObjectItem.Builder()
				.SetName(_objectInfo.GetName())
				.SetOriginalValue(originalValue)
				.SetNewValue(newValue);

			if (!_objectInfo.IsEqual(originalValue, newValue))
			{
				patchInfoBuilder.HasChanges();
				_objectInfo.Set(originalInstance, newValue);
			}

			yield return patchInfoBuilder.Build();
		}
	}

	[DebuggerDisplay("Complex Object Patch: {_objectInfo.GetName()}")]
	public class ComplexObjectPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;
		private readonly IEnumerable<IPatchInfo<TObject>> _patches;

		public ComplexObjectPatch(ObjectInfo<TInstance, TObject> objectInfo, IEnumerable<IPatchInfo<TObject>> patches)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
			_patches = patches;
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
				yield break;

			if (targetInstance == null)
				yield break;

			var originalValue = _objectInfo.Get(originalInstance);
			var newValue = _objectInfo.Get(targetInstance);
			
			foreach (var patchInfo in _patches)
			{
				foreach (var objectItem in patchInfo.Patch(originalValue, newValue))
				{
					yield return new ObjectItem.Builder(objectItem)
						.SetPrefix(_objectInfo.GetName())
						.Build();
				}
			}
		}
	}
}