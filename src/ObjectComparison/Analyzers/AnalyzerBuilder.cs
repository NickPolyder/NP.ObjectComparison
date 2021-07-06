using System.Collections.Generic;

namespace ObjectComparison.Analyzers
{
	public class AnalyzerBuilder<TInstance>
	{
		public static IEnumerable<IObjectAnalyzer<TInstance>> Build(TypeGeneratorBuilderOptions options = null)
		{
			var localOptions = options ?? new TypeGeneratorBuilderOptions();
			var publicProperties = typeof(TInstance).GetProperties();
			
			foreach (var publicProperty in publicProperties)
			{
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