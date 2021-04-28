using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher.Patch
{
	public class PatchComposite<TInstance> : IPatchInfo<TInstance>
	{
		private readonly IPatchInfo<TInstance>[] _patchInfos;

		public PatchComposite(params IPatchInfo<TInstance>[] patchInfos)
		{
			_patchInfos = patchInfos;
		}

		public IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{
			foreach (var patchInfo in _patchInfos)
			{
				foreach (var patchItem in patchInfo.Patch(originalInstance, targetInstance))
				{
					yield return patchItem;
				}
			}
		}
	}
}