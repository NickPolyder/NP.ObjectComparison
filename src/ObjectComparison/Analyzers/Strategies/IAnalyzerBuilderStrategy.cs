using System.Reflection;

namespace ObjectComparison.Analyzers.Strategies
{
	public interface IAnalyzerBuilderStrategy
	{
		IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options = null);
	}
}