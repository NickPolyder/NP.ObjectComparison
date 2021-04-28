using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher.Patch
{
	public interface IPatchInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance);
	}
}