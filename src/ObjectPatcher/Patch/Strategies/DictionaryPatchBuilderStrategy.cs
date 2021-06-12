using System.Reflection;

namespace ObjectPatcher.Patch.Strategies
{
	public class DictionaryPatchBuilderStrategy : IPatchBuilderStrategy
	{
		public IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo)
		{
			var genericParameters = propertyInfo.PropertyType.GetGenericArguments();

			var keyType = genericParameters[0];

			var valueType = genericParameters[1];

			var objectInfo = DictionaryObjectInfoBuilder<TInstance>.Build(propertyInfo);
			var propertyPatchType =
				typeof(DictionaryPatch<,,>)
					.MakeGenericType(typeof(TInstance), keyType, valueType)
					.GetConstructors()[0];

			return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
		}
	}
}