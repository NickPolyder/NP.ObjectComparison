using System;
using System.Reflection;
using ObjectComparison.Analyzers.Infos;

namespace ObjectComparison.Analyzers.Strategies
{
	public class ObjectAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var objectInfo = ObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object)
			{
				return GetComplexAnalyzer<TInstance>(propertyInfo, options, objectInfo);
			}

			var constructorInfo =
				typeof(ObjectAnalyzer<,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
					.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>)constructorInfo.Invoke(new[] { objectInfo });
		}

		private static IObjectAnalyzer<TInstance> GetComplexAnalyzer<TInstance>(PropertyInfo propertyInfo, 
			TypeGeneratorBuilderOptions options,
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

			var builder = typeof(AnalyzerBuilder<>).MakeGenericType(propertyInfo.PropertyType);

			var analyzers = builder.GetMethod(nameof(AnalyzerBuilder<TInstance>.Build))
				.Invoke(null, new object[] {options});

			var constructorInfo =
				typeof(ComplexObjectAnalyzer<,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
					.GetConstructors()[0];

			return (IObjectAnalyzer<TInstance>) constructorInfo.Invoke(new[] { objectInfo, analyzers });
		}
	}
}