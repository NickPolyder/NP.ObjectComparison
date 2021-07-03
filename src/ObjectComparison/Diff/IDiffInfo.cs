using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Diff
{
	public interface IDiffInfo<in TInstance>
	{
		IEnumerable<ObjectItem> Diff(TInstance originalInstance, TInstance targetInstance);
	}
}