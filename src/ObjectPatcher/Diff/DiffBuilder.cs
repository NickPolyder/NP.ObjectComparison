using System.Collections.Generic;
using ObjectPatcher.Diff.Strategies;

namespace ObjectPatcher.Diff
{
	public class DiffBuilder<TInstance>
	{
		private static readonly IDiffBuilderStrategy _objectBuilderStrategy = new ObjectDiffBuilderStrategy();
		private static readonly IDiffBuilderStrategy _arrayBuilderStrategy = new ArrayDiffBuilderStrategy();
		public static IEnumerable<IDiffInfo<TInstance>> Build()
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