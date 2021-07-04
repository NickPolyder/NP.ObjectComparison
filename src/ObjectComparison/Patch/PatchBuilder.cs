using System.Collections.Generic;

namespace ObjectComparison.Patch
{
	public class PatchBuilder<TInstance>
	{
		public static IEnumerable<IPatchInfo<TInstance>> Build(TypeGeneratorBuilderOptions options = null)
		{
			var localOptions = options ?? new TypeGeneratorBuilderOptions();
			var publicProperties = typeof(TInstance).GetProperties();
			
			foreach (var publicProperty in publicProperties)
			{
				if (publicProperty.PropertyType.HasInterface(Constants.DictionaryInterfaceType))
				{
					var dictionaryPatch = PatchSingletons.DictionaryBuilderStrategy.Build<TInstance>(publicProperty);
					if (dictionaryPatch != null)
					{
						yield return dictionaryPatch;
					}
					continue;
				}

				if (publicProperty.PropertyType.IsCollectionType())
				{
					var arrayPatch = PatchSingletons.ArrayBuilderStrategy.Build<TInstance>(publicProperty);
					if (arrayPatch != null)
					{
						yield return arrayPatch;
					}
					continue;
				}
				
				var objectPatch =  PatchSingletons.ObjectBuilderStrategy.Build<TInstance>(publicProperty, localOptions);
				if (objectPatch != null)
				{
					yield return objectPatch;
				}
			}
		}
	}
}