using System.Collections;
using System.Collections.Generic;
using ObjectPatcher.Patch.Strategies;

namespace ObjectPatcher.Patch
{
	public class PatchBuilder<TInstance>
	{
		private static IPatchBuilderStrategy _objectBuilderStrategy = new ObjectPatchBuilderStrategy();
		private static IPatchBuilderStrategy _arrayPatchBuilderStrategy = new ArrayPatchBuilderStrategy();
		public static IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				if (typeof(IEnumerable<>).IsAssignableFrom(publicProperty.PropertyType)
				    || typeof(IEnumerable).IsAssignableFrom(publicProperty.PropertyType))
				{
					yield return _arrayPatchBuilderStrategy.Build<TInstance>(publicProperty);
					continue;
				}
				

				yield return _objectBuilderStrategy.Build<TInstance>(publicProperty);
			}
		}
	}
}