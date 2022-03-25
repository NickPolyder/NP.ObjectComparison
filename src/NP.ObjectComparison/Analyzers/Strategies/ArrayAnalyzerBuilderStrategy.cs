using System;
using System.Reflection;
using NP.ObjectComparison.Analyzers.Infos;
using NP.ObjectComparison.Analyzers.Settings;

namespace NP.ObjectComparison.Analyzers.Strategies
{
	/// <summary>
	/// The strategy that builds: <see cref="ArrayAnalyzer{TInstance, TArray, TArrayOf}"/>.
	/// </summary>
	public class ArrayAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		/// <summary>
		/// The strategy that builds: <see cref="ArrayAnalyzer{TInstance, TArray, TArrayOf}"/>.
		/// </summary>
		/// <typeparam name="TInstance">The instance this analyzer is for.</typeparam>
		/// <param name="propertyInfo">The property of the <typeparamref name="TInstance"/> to be analyzed.</param>
		/// <param name="options">Options related to this analyzer.</param>
		/// <returns>The <see cref="ArrayAnalyzer{TInstance, TArray, TArrayOf}"/>.</returns>
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options)
		{
			var arrayOf = propertyInfo.PropertyType.GetCollectionElementType();

			var objectInfo = ArrayObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(arrayOf) == TypeCode.Object && !arrayOf.IsNullable())
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
			AnalyzerSettings options,
			Type arrayOf,
			object objectInfo)
		{
			if (options?.Depth != null)
			{
				if (options.Depth.IsAllowedDepth(propertyInfo))
				{
					return null;
				}

				options.Depth.IncreaseDepth(propertyInfo);
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