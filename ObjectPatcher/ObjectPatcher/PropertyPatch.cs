using System;

namespace ObjectPatcher
{
	public class PropertyPatch : IPatchInfo
	{
		private readonly Func<object, object> _getterFunction;
		private readonly Action<object, object> _setterAction;
		private readonly EqualsPredicate<object> _equalsPredicate;

		public PropertyPatch(Func<object, object> getterFunction, Action<object, object> setterAction)
		: this(getterFunction, setterAction, object.Equals)
		{  }

		public PropertyPatch(Func<object, object> getterFunction, Action<object, object> setterAction, EqualsPredicate<object> equalsPredicate)
		{
			_getterFunction = getterFunction ?? throw new ArgumentNullException(nameof(getterFunction));
			_setterAction = setterAction ?? throw new ArgumentNullException(nameof(setterAction));
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		public bool Patch(object originalInstance, object targetInstance)
		{
			var originalValue = Get(originalInstance);
			var newValue = Get(targetInstance);
			if (IsEqual(originalValue, newValue))
			{
				return false;
			}

			Set(originalInstance, newValue);
			
			return true;
		}

		private object Get(object instance)
		{
			return _getterFunction(instance);
		}

		private void Set(object originalInstance, object targetValue)
		{
			_setterAction(originalInstance, targetValue);
		}

		private bool IsEqual(object originalValue, object targetValue)
		{
			return _equalsPredicate(originalValue, targetValue);
		}
	}
}