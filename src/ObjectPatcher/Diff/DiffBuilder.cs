using System.Collections.Generic;

namespace ObjectPatcher.Diff
{
	public class DiffBuilder<TInstance>
	{
		public static IEnumerable<IDiffInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				var objectInfo = ObjectInfoBuilder<TInstance>.Build(publicProperty);

				var propertyDiffType =
					typeof(ObjectDiff<,>)
						.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
						.GetConstructors()[0];

				yield return (IDiffInfo<TInstance>)propertyDiffType.Invoke(new[] { objectInfo });
			}
		}
	}
}