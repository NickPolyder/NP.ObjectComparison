using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Patch
{
	public interface IPatchInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Patch(TInstance originalInstance, TInstance targetInstance);
	}
}