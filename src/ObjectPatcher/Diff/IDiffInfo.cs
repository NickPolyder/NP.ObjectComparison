using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher.Diff
{
	public interface IDiffInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance);
	}
}