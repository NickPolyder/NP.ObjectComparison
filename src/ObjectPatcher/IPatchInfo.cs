using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher
{
	public interface IPatchInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance);
	}
}