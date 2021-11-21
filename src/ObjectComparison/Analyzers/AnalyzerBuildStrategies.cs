using ObjectComparison.Analyzers.Strategies;

namespace ObjectComparison.Analyzers
{
	/// <summary>
	/// All the available build analyzing strategies as a singleton.
	/// </summary>
	public static class AnalyzerBuildStrategies
	{
		/// <summary>
		/// <see cref="ObjectAnalyzerBuilderStrategy"/>.
		/// </summary>
		public static readonly IAnalyzerBuilderStrategy ObjectBuilderStrategy = new ObjectAnalyzerBuilderStrategy();

		/// <summary>
		/// <see cref="ArrayAnalyzerBuilderStrategy"/>.
		/// </summary>
		public static readonly IAnalyzerBuilderStrategy ArrayBuilderStrategy = new ArrayAnalyzerBuilderStrategy();

		/// <summary>
		/// <see cref="DictionaryAnalyzerBuilderStrategy"/>.
		/// </summary>
		public static readonly IAnalyzerBuilderStrategy DictionaryBuilderStrategy = new DictionaryAnalyzerBuilderStrategy();

	}
}