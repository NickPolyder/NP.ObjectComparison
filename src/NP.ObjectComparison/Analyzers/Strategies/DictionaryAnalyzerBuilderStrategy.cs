using System;
using System.Reflection;
using NP.ObjectComparison.Analyzers.Infos;
using NP.ObjectComparison.Analyzers.Settings;

namespace NP.ObjectComparison.Analyzers.Strategies
{
	/// <summary>
	/// The strategy that builds: <see cref="DictionaryAnalyzer{TInstance, TKey, TValue}"/>.
	/// </summary>
	public class DictionaryAnalyzerBuilderStrategy : IAnalyzerBuilderStrategy
	{
		/// <summary>
		/// The strategy that builds: <see cref="DictionaryAnalyzer{TInstance, TKey, TValue}"/>.
		/// </summary>
		/// <typeparam name="TInstance">The instance this analyzer is for.</typeparam>
		/// <param name="propertyInfo">The property of the <typeparamref name="TInstance"/> to be analyzed.</param>
		/// <param name="options">Options related to this analyzer.</param>
		/// <returns>The <see cref="DictionaryAnalyzer{TInstance, TKey, TValue}"/>.</returns>
		public IObjectAnalyzer<TInstance> Build<TInstance>(PropertyInfo propertyInfo, AnalyzerSettings options)
		{
			var genericParameters = propertyInfo.PropertyType.GetGenericArguments();

			var keyType = genericParameters[0];

			var valueType = genericParameters[1];

			var objectInfo = DictionaryObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(valueType) == TypeCode.Object && !valueType.IsNullable())
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
			if (options?.Depth != null)
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