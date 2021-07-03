using System.Reflection;

namespace ObjectComparison.Diff.Strategies
{
	public class ArrayDiffBuilderStrategy : IDiffBuilderStrategy
	{
		public IDiffInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo)
		{
			var arrayOf = propertyInfo.PropertyType.GetCollectionElementType();

			var objectInfo = ArrayObjectInfoBuilder<TInstance>.Build(propertyInfo);
				var propertyPatchType =
				typeof(ArrayDiff<,,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType, arrayOf)
					.GetConstructors()[0];

			return (IDiffInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}