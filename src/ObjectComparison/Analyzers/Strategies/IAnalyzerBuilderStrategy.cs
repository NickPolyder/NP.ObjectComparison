using System.Reflection;
using ObjectComparison.Analyzers.Settings;

namespace ObjectComparison.Analyzers.Strategies
{
	public interface IAnalyzerBuilderStrategy
	{
		IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options = null);
	}
}