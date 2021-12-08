using System.Reflection;
using NP.ObjectComparison.Analyzers.Settings;

namespace NP.ObjectComparison.Analyzers.Strategies
{
	/// <summary>
	/// A strategy builder for analyzers.
	/// </summary>
	public interface IAnalyzerBuilderStrategy
	{
		/// <summary>
		/// The strategy that builds a  concrete <see cref="IObjectAnalyzer{TInstance}"/>.
		/// </summary>
		/// <typeparam name="TInstance">The instance this analyzer is for.</typeparam>
		/// <param name="propertyInfo">The property of the <typeparamref name="TInstance"/> to be analyzed.</param>
		/// <param name="options">Options related to this analyzer.</param>
		/// <returns>The concrete version of <see cref="IObjectAnalyzer{TInstance}"/>.</returns>
		IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options = null);
	}
}