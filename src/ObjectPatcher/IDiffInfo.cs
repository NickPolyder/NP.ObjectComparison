using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher
{
	public interface IDiffInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance);
	}
}