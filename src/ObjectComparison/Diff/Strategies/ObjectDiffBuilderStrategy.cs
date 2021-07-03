using System.Reflection;

namespace ObjectComparison.Diff.Strategies
{
	public class ObjectDiffBuilderStrategy: IDiffBuilderStrategy
	{
		public IDiffInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo)
		{
			var objectInfo = ObjectInfoBuilder<TInstance>.Build(propertyInfo);

			var propertyPatchType =
				typeof(ObjectDiff<,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
					.GetConstructors()[0];

			return (IDiffInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}