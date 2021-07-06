using ObjectComparison.Results;

namespace ObjectComparison
{
	public interface IObjectAnalyzer<in TInstance>
	{
		IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance);
	}
}