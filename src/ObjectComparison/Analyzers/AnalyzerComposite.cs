using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	public class AnalyzerComposite<TInstance> : IObjectAnalyzer<TInstance>
	{
		private readonly IObjectAnalyzer<TInstance>[] _analyzers;

		public AnalyzerComposite(params IObjectAnalyzer<TInstance>[] analyzers)
		{
			_analyzers = analyzers;
		}

		public IEnumerable<DiffSnapshot> Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			foreach (var analyzer in _analyzers)
			{
				foreach (var analyzedItem in analyzer.Analyze(originalInstance, targetInstance))
				{
					yield return analyzedItem;
				}
			}
		}
	}
}