using System;
using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
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
			{
				yield break;
			}

			if (targetInstance == null)
			{
				yield break;
			}

			var originalValue = _objectInfo.Get(originalInstance);
			var targetValue = _objectInfo.Get(targetInstance);
			
			foreach (var patchInfo in _patches)
			{
				foreach (var objectItem in patchInfo.Patch(originalValue, targetValue))
				{
					yield return new ObjectItem.Builder(objectItem)
						.SetPrefix(_objectInfo.GetName())
						.Build();
				}
			}
		}
	}
}