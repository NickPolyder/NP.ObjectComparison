using System;
using System.Reflection;
using ObjectComparison.Analyzers.Infos;

namespace ObjectComparison.Analyzers.Strategies
{
	public class ArrayAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var arrayOf = propertyInfo.PropertyType.GetCollectionElementType();

			var objectInfo = ArrayObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(arrayOf) == TypeCode.Object)
			{
				return GetComplexAnalyzer<TInstance>(propertyInfo, options, arrayOf, objectInfo);
			}

			var constructorInfo =
			typeof(ArrayAnalyzer<,,>)
				.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
				.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>)constructorInfo.Invoke(new[] { objectInfo });
		}
		
		private static IObjectAnalyzer<TInstance> GetComplexAnalyzer<TInstance>(PropertyInfo propertyInfo,
			TypeGeneratorBuilderOptions options,
			Type arrayOf,
			object objectInfo)
		{
			if (options != null)
			{
				if (options.IsAllowedDepth(propertyInfo))
				{
					return null;
				}

				options.Depth[propertyInfo]++;
			}

			var builder = typeof(AnalyzerBuilder<>).MakeGenericType(arrayOf);

			var analyzers = builder.GetMethod(nameof(AnalyzerBuilder<TInstance>.Build))
				.Invoke(null, new object[] { options });

			var constructorInfo =
				typeof(ComplexArrayAnalyzer<,,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
					.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>)constructorInfo.Invoke(new[] { objectInfo, analyzers });
		}
	}
}