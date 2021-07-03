using System;
using System.Collections.Generic;
using ObjectComparison.Patch.Strategies;

namespace ObjectComparison.Patch
{
	public class PatchBuilder<TInstance>
	{
		private static readonly Type _dictionaryInterface = typeof(IDictionary<,>);
		private static readonly IPatchBuilderStrategy _objectBuilderStrategy = new ObjectPatchBuilderStrategy();
		private static readonly IPatchBuilderStrategy _arrayBuilderStrategy = new ArrayPatchBuilderStrategy();
		private static readonly IPatchBuilderStrategy _dictionaryBuilderStrategy = new DictionaryPatchBuilderStrategy();
		public static IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				if (publicProperty.PropertyType.HasInterface(_dictionaryInterface))
				{
					yield return _dictionaryBuilderStrategy.Build<TInstance>(publicProperty);
					continue;
				}

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