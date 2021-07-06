using ObjectComparison.Analyzers.Strategies;

namespace ObjectComparison.Analyzers
{
	public static class AnalyzerSingletons
	{
		public static readonly IAnalyzerBuilderStrategy ObjectBuilderStrategy = new ObjectAnalyzerBuilderStrategy();
		public static readonly IAnalyzerBuilderStrategy ArrayBuilderStrategy = new ArrayAnalyzerBuilderStrategy();
		public static readonly IAnalyzerBuilderStrategy DictionaryBuilderStrategy = new DictionaryAnalyzerBuilderStrategy();

	}
}