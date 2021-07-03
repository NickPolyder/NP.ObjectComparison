using System.Reflection;

namespace ObjectComparison.Diff
{
	public interface IDiffBuilderStrategy
	{
		IDiffInfo<TInstance> Build<TInstance>(PropertyInfo propertyInfo);
	}
}