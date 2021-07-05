using System;
using System.Reflection;

namespace ObjectComparison.Patch.Strategies
{
	public class ArrayPatchBuilderStrategy : IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var arrayOf = propertyInfo.PropertyType.GetCollectionElementType();

			var objectInfo = ArrayObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(arrayOf) == TypeCode.Object)
			{
				return GetComplexPatch<TInstance>(propertyInfo, options, arrayOf, objectInfo);
			}

			var propertyPatchType =
			typeof(ArrayPatch<,,>)
				.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
				.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
		
		private static IPatchInfo<TInstance> GetComplexPatch<TInstance>(PropertyInfo propertyInfo,
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

			var specificPatchBuilder = typeof(PatchBuilder<>).MakeGenericType(arrayOf);

			var patches = specificPatchBuilder.GetMethod(nameof(PatchBuilder<TInstance>.Build))
				.Invoke(null, new object[] { options });

			var complexPatchType =
				typeof(ComplexArrayPatch<,,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)complexPatchType.Invoke(new[] { objectInfo, patches });
		}
	}
}