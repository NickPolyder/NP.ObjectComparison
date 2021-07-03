using System.Reflection;

namespace ObjectComparison.Patch.Strategies
{
	public class ArrayPatchBuilderStrategy : IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options)
		{
			var arrayOf = propertyInfo.PropertyType.GetCollectionElementType();

			var objectInfo = ArrayObjectInfoBuilder<TInstance>.Build(propertyInfo);
			var propertyPatchType =
			typeof(ArrayPatch<,,>)
				.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
				.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}