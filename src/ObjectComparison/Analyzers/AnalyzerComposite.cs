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

		public IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			var diffAnalysisResult = new DiffAnalysisResult();

			foreach (var analyzer in _analyzers)
			{
				diffAnalysisResult.Merge(analyzer.Analyze(originalInstance, targetInstance));
			}

			return diffAnalysisResult;
		}
	}
}