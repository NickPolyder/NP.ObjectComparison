using System.Reflection;

namespace ObjectPatcher.Diff
{
	public interface IDiffBuilderStrategy
	{
		IDiffInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo);
	}
}