using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Diff
{
	public class DiffComposite<TInstance> : IDiffInfo<TInstance>
	{
		private readonly IDiffInfo<TInstance>[] _diffInfos;

		public DiffComposite(params IDiffInfo<TInstance>[] diffInfos)
		{
			_diffInfos = diffInfos;
		}

		public IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance)
		{
			foreach (var patchInfo in _diffInfos)
			{
				foreach (var patchItem in patchInfo.Diff(originalInstance, targetInstance))
				{
					yield return patchItem;
				}
			}
		}
	}
}