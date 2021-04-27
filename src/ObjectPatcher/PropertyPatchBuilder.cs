using System.Collections.Generic;

namespace ObjectPatcher
{
	public class PropertyPatchBuilder<TInstance>
	{
		public static IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				var objectInfo = ObjectInfoBuilder<TInstance>.Build(publicProperty);

				var propertyPatchType =
					typeof(PropertyPatch<,>)
						.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
						.GetConstructors()[0];

				yield return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
			}
		}
	}

	public class PropertyDiffBuilder<TInstance>
	{
		public static IEnumerable<IDiffInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				var objectInfo = ObjectInfoBuilder<TInstance>.Build(publicProperty);

				var propertyDiffType =
					typeof(PropertyDiff<,>)
						.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
						.GetConstructors()[0];

				yield return (IDiffInfo<TInstance>)propertyDiffType.Invoke(new[] { objectInfo });
			}
		}
	}
}