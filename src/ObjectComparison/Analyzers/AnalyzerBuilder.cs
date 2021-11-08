using System;
using System.Collections.Generic;
using ObjectComparison.Analyzers.Settings;

namespace ObjectComparison.Analyzers
{
	public class AnalyzerBuilder<TInstance>
	{
		public static IEnumerable<IObjectAnalyzer<TInstance>> Build(AnalyzerSettings options = null)
		{
			var localOptions = options 
			                   ?? AnalyzerSettings.DefaultSettings.Invoke() 
			                   ?? new AnalyzerSettings();

			var typeToAnalyze = typeof(TInstance);

			if (localOptions.SkipAnalyzeSettings.IsSkipped(typeToAnalyze))
			{
				yield break;
			}

			var publicProperties = typeToAnalyze.GetProperties();
			
			foreach (var publicProperty in publicProperties)
			{
				if (localOptions.SkipAnalyzeSettings.IsSkipped(publicProperty))
				{
					continue;
				}

				if (publicProperty.PropertyType.HasInterface(Constants.DictionaryInterfaceType))
				{
					var analyzer = AnalyzerSingletons.DictionaryBuilderStrategy.Build<TInstance>(publicProperty);
					if (analyzer != null)
					{
						yield return analyzer;
					}

					continue;
				}

				if (publicProperty.PropertyType.IsCollectionType())
				{
					var analyzer = AnalyzerSingletons.ArrayBuilderStrategy.Build<TInstance>(publicProperty);
					if (analyzer != null)
					{
						yield return analyzer;
					}

					continue;
				}
				
				var objectAnalyzer =  AnalyzerSingletons.ObjectBuilderStrategy.Build<TInstance>(publicProperty, localOptions);
				if (objectAnalyzer != null)
				{
					yield return objectAnalyzer;
				}
			}
		}
	}
}