using System;
using System.Reflection;

namespace ObjectComparison.Patch.Strategies
{
	public class DictionaryPatchBuilderStrategy : IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var genericParameters = propertyInfo.PropertyType.GetGenericArguments();

			var keyType = genericParameters[0];

			var valueType = genericParameters[1];

			var objectInfo = DictionaryObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(valueType) == TypeCode.Object)
			{
				return GetComplexPatch<TInstance>(propertyInfo, options, keyType, valueType, objectInfo);
			}

			var propertyPatchType =
				typeof(DictionaryPatch<,,>)
					.MakeGenericType(typeof(TInstance), keyType, valueType)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}

		private static IPatchInfo<TInstance> GetComplexPatch<TInstance>(PropertyInfo propertyInfo,
			TypeGeneratorBuilderOptions options,
			Type keyType, 
			Type valueType,
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

			var specificPatchBuilder = typeof(PatchBuilder<>).MakeGenericType(valueType);

			var patches = specificPatchBuilder.GetMethod(nameof(PatchBuilder<TInstance>.Build))
				.Invoke(null, new object[] { options });

			var complexPatchType =
				typeof(ComplexDictionaryPatch<,,>)
					.MakeGenericType(typeof(TInstance), keyType, valueType)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)complexPatchType.Invoke(new[] { objectInfo, patches });
		}
	}
}