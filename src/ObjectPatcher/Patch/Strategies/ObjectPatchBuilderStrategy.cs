using System.Reflection;

namespace ObjectPatcher.Patch.Strategies
{
	public class ObjectPatchBuilderStrategy: IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo)
		{
			var objectInfo = ObjectInfoBuilder<TInstance>.Build(propertyInfo);

			var propertyPatchType =
				typeof(ObjectPatch<,>)
					.MakeGenericType(typeof(TInstance), propertyInfo.PropertyType)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}