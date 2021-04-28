using System.Collections.Generic;

namespace ObjectPatcher.Patch
{
	public class PatchBuilder<TInstance>
	{
		public static IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
			{
				var objectInfo = ObjectInfoBuilder<TInstance>.Build(publicProperty);

				var propertyPatchType =
					typeof(ObjectPatch<,>)
						.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
						.GetConstructors()[0];

				yield return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new[] { objectInfo });
			}
		}
	}
}