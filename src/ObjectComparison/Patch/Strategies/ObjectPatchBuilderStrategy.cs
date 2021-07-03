using System;
using System.Reflection;

namespace ObjectComparison.Patch.Strategies
{
	public class ObjectPatchBuilderStrategy : IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var objectInfo = ObjectInfoBuilder<TInstance>.Build(propertyInfo);

			if (Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object)
			{
				var depth = (options?.Depth.GetOrAdd(propertyInfo, key => 0)).GetValueOrDefault(0);
				var maxDepth = (options?.MaxDepth).GetValueOrDefault(Constants.MaxReferenceLoopDepth);

				if (depth >= maxDepth)
				{
					return null;
				}

				if (options != null)
				{
					options.Depth[propertyInfo]++;
				}

				var specificPatchBuilder = typeof(PatchBuilder<>).MakeGenericType(propertyInfo.PropertyType);

				var patches = specificPatchBuilder.GetMethod(nameof(PatchBuilder<TInstance>.Build)).Invoke(null, new object[] { options });

				var complexPatchType =
					typeof(ComplexObjectPatch<,>)
						.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
						.GetConstructors()[0];

				return (IPatchInfo<TInstance>)complexPatchType.Invoke(new[] { objectInfo, patches });
			}

			var propertyPatchType =
				typeof(ObjectPatch<,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}