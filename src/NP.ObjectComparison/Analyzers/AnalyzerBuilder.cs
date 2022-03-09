using System.Collections.Generic;
using NP.ObjectComparison.Analyzers.Settings;

namespace NP.ObjectComparison.Analyzers
{
	/// <summary>
	/// A generic analyzer builder
	/// </summary>
	/// <typeparam name="TInstance">The object type to be analyzed.</typeparam>
	public class AnalyzerBuilder<TInstance>
	{
		/// <summary>
		/// Builds an Analyzer for the <typeparamref name="TInstance"/>.
		/// </summary>
		/// <param name="options">The options for this build.</param>
		/// <returns>A built Analyzer.</returns>
		public static IEnumerable<IObjectAnalyzer<TInstance>> Build(AnalyzerSettings options = null)
		{
			var localOptions = options 
			                   ?? AnalyzerSettings.DefaultSettings.Invoke() 
			                   ?? new AnalyzerSettings();

			var typeToAnalyze = typeof(TInstance);

			if (localOptions.IgnoreSettings.IsIgnored(typeToAnalyze))
			{
				yield break;
			}

			var publicProperties = typeToAnalyze.GetProperties();
			
			foreach (var publicProperty in publicProperties)
			{
				if (localOptions.IgnoreSettings.IsIgnored(publicProperty))
				{
					continue;
				}

				if (publicProperty.PropertyType.HasInterface(Constants.DictionaryInterfaceType))
				{
					var analyzer = AnalyzerBuildStrategies.DictionaryBuilderStrategy.Build<TInstance>(publicProperty);
					if (analyzer != null)
					{
						yield return analyzer;
					}

					continue;
				}

				if (publicProperty.PropertyType.IsCollectionType())
				{
					var analyzer = AnalyzerBuildStrategies.ArrayBuilderStrategy.Build<TInstance>(publicProperty);
					if (analyzer != null)
					{
						yield return analyzer;
					}

					continue;
				}
				
				var objectAnalyzer =  AnalyzerBuildStrategies.ObjectBuilderStrategy.Build<TInstance>(publicProperty, localOptions);
				if (objectAnalyzer != null)
				{
					yield return objectAnalyzer;
				}
			}
		}
	}
}