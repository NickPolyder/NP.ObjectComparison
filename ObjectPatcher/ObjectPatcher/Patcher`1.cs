using System;
using System.Linq;

namespace ObjectPatcher
{
	public class Patcher<TObject>: IPatchInfo<TObject>
	{
		private readonly PropertyPatch[] _propertyPatches;

		public Patcher(): this(typeof(TObject).CreatePropertyPatch().ToArray())
		{
		}

		public Patcher(params PropertyPatch[] propertyPatches)
		{
			_propertyPatches = propertyPatches ?? throw new ArgumentNullException(nameof(propertyPatches));
		}

		public bool Patch(TObject originalInstance, TObject targetInstance)
		{
			bool hasBeenPatched = false;
			foreach (var propertyPatch in _propertyPatches)
			{
				hasBeenPatched = propertyPatch.Patch(originalInstance, targetInstance);
			}

			return hasBeenPatched;
		}
	}
}