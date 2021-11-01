using System;
using System.Reflection;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Analyzers.Settings;

namespace ObjectComparison.Analyzers.Strategies
{
	public class DictionaryAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options)
		{
			var genericParameters = propertyInfo.PropertyType.GetGenericArguments();

			var keyType = genericParameters[0];

			var valueType = genericParameters[1];

			var objectInfo = DictionaryObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(valueType) == TypeCode.Object)
			{
				return GetComplexAnalyzer<TInstance>(propertyInfo, options, keyType, valueType, objectInfo);
			}

			var constructorInfo =
				typeof(DictionaryAnalyzer<,,>)
					.MakeGenericType(typeof(TInstance), keyType, valueType)
					.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>)constructorInfo.Invoke(new[] { objectInfo });
		}

		private static IObjectAnalyzer<TInstance> GetComplexAnalyzer<TInstance>(PropertyInfo propertyInfo,
			AnalyzerSettings options,
			Type keyType, 
			Type valueType,
			object objectInfo)
		{
			if (options != null)
			{
				if (options.Depth.IsAllowedDepth(propertyInfo))
				{
					return null;
				}

				options.Depth.IncreaseDepth(propertyInfo);
			}

			var builder = typeof(AnalyzerBuilder<>).MakeGenericType(valueType);

			var analyzers = builder.GetMethod(nameof(AnalyzerBuilder<TInstance>.Build))
				.Invoke(null, new object[] { options });

			var constructorInfo =
				typeof(ComplexDictionaryAnalyzer<,,>)
					.MakeGenericType(typeof(TInstance), keyType, valueType)
					.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>)constructorInfo.Invoke(new[] { objectInfo, analyzers });
		}
	}
}