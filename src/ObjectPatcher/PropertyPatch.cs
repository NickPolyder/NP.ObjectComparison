using System;

namespace ObjectPatcher
{
	public class PropertyPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly Func<TInstance, TObject> _getterFunction;
		private readonly Action<TInstance, TObject> _setterAction;
		private readonly EqualsPredicate<TObject> _equalsPredicate;

		public PropertyPatch(Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction)
		: this(getterFunction, setterAction, (origin, target) => origin.Equals(target))
		{ }

		public PropertyPatch(Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction, EqualsPredicate<TObject> equalsPredicate)
		{
			_getterFunction = getterFunction ?? throw new ArgumentNullException(nameof(getterFunction));
			_setterAction = setterAction ?? throw new ArgumentNullException(nameof(setterAction));
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		public bool Patch(TInstance originalInstance, TInstance targetInstance)
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

		private TObject Get(TInstance instance)
		{
			return _getterFunction(instance);
		}

		private void Set(TInstance originalInstance, TObject targetValue)
		{
			_setterAction(originalInstance, targetValue);
		}

		private bool IsEqual(TObject originalValue, TObject targetValue)
		{
			return _equalsPredicate(originalValue, targetValue);
		}
	}
}