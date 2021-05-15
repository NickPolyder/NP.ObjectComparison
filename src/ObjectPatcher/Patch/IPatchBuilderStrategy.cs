using System.Reflection;

namespace ObjectPatcher.Patch
{
	public interface IPatchBuilderStrategy
	{
		IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo);
	}
}