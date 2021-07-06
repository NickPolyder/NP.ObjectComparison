using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison
{
	public interface IObjectAnalyzer<in TInstance>
	{
		IEnumerable<DiffSnapshot> Analyze(TInstance originalInstance, TInstance targetInstance);
	}
}