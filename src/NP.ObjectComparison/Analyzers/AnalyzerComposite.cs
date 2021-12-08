using NP.ObjectComparison.Results;

namespace NP.ObjectComparison.Analyzers
{
	/// <summary>
	/// Holds a list of analyzers for a specific <typeparamref name="TInstance"/>.
	/// </summary>
	/// <typeparam name="TInstance">The object type to be analyzed.</typeparam>
	public class AnalyzerComposite<TInstance> : IObjectAnalyzer<TInstance>
	{
		private readonly IObjectAnalyzer<TInstance>[] _analyzers;

		/// <summary>
		/// Construct a composite of <paramref name="analyzers"/>.
		/// </summary>
		/// <param name="analyzers"></param>
		public AnalyzerComposite(params IObjectAnalyzer<TInstance>[] analyzers)
		{
			_analyzers = analyzers;
		}

		/// <inheritdoc />
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