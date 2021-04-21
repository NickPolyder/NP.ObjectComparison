using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectPatcher
{
	public static class PatchExtensions
	{
		public static IEnumerable<PropertyPatch> CreatePropertyPatch(this Type type)
		{
			var publicProperties = type.GetProperties(BindingFlags.Public);

			foreach (var publicProperty in publicProperties)
			{
				var getterMethod = publicProperty.GetGetMethod();

				var getterFunc = new Func<object, object>(instance => getterMethod.Invoke(instance, Array.Empty<object>()));

				var setterMethod = publicProperty.GetSetMethod();

				var setterAction = new Action<object, object>((instance, value) => setterMethod.Invoke(instance, new[] { value }));

				yield return new PropertyPatch(getterFunc, setterAction);
			}
		}
	}
}