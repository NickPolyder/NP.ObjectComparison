using System.Reflection;
using ObjectPatcher.Patch;

namespace ObjectPatcher
{
	public interface IPatchBuilderStrategy
	{
		IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo);
	}
}