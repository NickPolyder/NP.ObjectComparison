using System.Collections.Generic;
using ObjectPatcher.Patch.Strategies;

namespace ObjectPatcher.Patch
{
	public class PatchBuilder<TInstance>
	{
		private static readonly IPatchBuilderStrategy _objectBuilderStrategy = new ObjectPatchBuilderStrategy();
		private static readonly IPatchBuilderStrategy _arrayBuilderStrategy = new ArrayPatchBuilderStrategy();
		public static IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				if (publicProperty.PropertyType.IsCollectionType())
				{
					yield return _arrayBuilderStrategy.Build<TInstance>(publicProperty);
					continue;
				}
				
				yield return _objectBuilderStrategy.Build<TInstance>(publicProperty);
			}
		}
	}
}