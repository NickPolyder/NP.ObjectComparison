using System.Reflection;

namespace ObjectComparison.Patch
{
	public interface IPatchBuilderStrategy
	{
		IPatchInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo, TypeGeneratorBuilderOptions options = null);
	}
}