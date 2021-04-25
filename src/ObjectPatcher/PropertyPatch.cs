using System;
using System.Collections.Generic;
using ObjectPatcher.Results;

namespace ObjectPatcher
{
	public class PropertyPatch<TInstance, TObject> : IPatchInfo<TInstance>
	{
		private readonly Func<string> _getNameFunction;
		private readonly Func<TInstance, TObject> _getterFunction;
		private readonly Action<TInstance, TObject> _setterAction;
		private readonly EqualsPredicate<TObject> _equalsPredicate;

		public PropertyPatch(Func<string> getNameFunction, Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction)
		: this(getNameFunction, getterFunction, setterAction, (origin, target) => origin.Equals(target))
		{ }

		public PropertyPatch(Func<string> getNameFunction, Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction, EqualsPredicate<TObject> equalsPredicate)
		{
			_getNameFunction = getNameFunction ?? throw new ArgumentNullException(nameof(getNameFunction));
			_getterFunction = getterFunction ?? throw new ArgumentNullException(nameof(getterFunction));
			_setterAction = setterAction ?? throw new ArgumentNullException(nameof(setterAction));
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		public IEnumerable<PatchItem> Patch(TInstance originalInstance, TInstance targetInstance)
		{

			var originalValue = Get(originalInstance);
			var newValue = Get(targetInstance);
			var patchInfoBuilder = new PatchItem.Builder()
				.SetName(GetName())
				.SetOriginalValue(originalValue);

			if (!IsEqual(originalValue, newValue))
			{
				Set(originalInstance, newValue);
				patchInfoBuilder
					.SetNewValue(newValue)
					.HasChanges();
			}

			return new[] { patchInfoBuilder.Build() };
		}

		private string GetName()
		{
			return _getNameFunction();
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