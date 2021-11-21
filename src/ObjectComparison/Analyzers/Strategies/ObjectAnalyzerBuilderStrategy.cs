using System;
using System.Reflection;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Analyzers.Settings;

namespace ObjectComparison.Analyzers.Strategies
{
	/// <summary>
	/// The strategy that builds: <see cref="ObjectAnalyzer{TInstance, TObject}"/>.
	/// </summary>
	public class ObjectAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		/// <summary>
		/// The strategy that builds: <see cref="ObjectAnalyzer{TInstance, TObject}"/>.
		/// </summary>
		/// <typeparam name="TInstance">The instance this analyzer is for.</typeparam>
		/// <param name="propertyInfo">The property of the <typeparamref name="TInstance"/> to be analyzed.</param>
		/// <param name="options">Options related to this analyzer.</param>
		/// <returns>The <see cref="ObjectAnalyzer{TInstance, TObject}"/>.</returns>
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options)
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
			AnalyzerSettings options,
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