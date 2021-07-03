using System;
using System.Collections.Generic;

namespace ObjectComparison
{
	public class DictionaryObjectInfo<TInstance, TKey, TValue>
	{
		private readonly Func<string> _getNameFunction;
		private readonly Func<TInstance, IDictionary<TKey, TValue>> _getterFunction;
		private readonly Action<TInstance, IDictionary<TKey, TValue>> _setterAction;
		private EqualsPredicate<TKey> _equalsKeyPredicate;
		private EqualsPredicate<TValue> _equalsValuePredicate;

		public DictionaryObjectInfo(Func<string> getNameFunction,
			Func<TInstance, IDictionary<TKey, TValue>> getterFunction,
			Action<TInstance, IDictionary<TKey, TValue>> setterAction)
			: this(getNameFunction, getterFunction, setterAction,
				(origin, target) => origin.Equals(target),
				(origin, target) => origin.Equals(target))
		{ }

		public DictionaryObjectInfo(Func<string> getNameFunction,
			Func<TInstance, IDictionary<TKey, TValue>> getterFunction,
			Action<TInstance, IDictionary<TKey, TValue>> setterAction,
			EqualsPredicate<TKey> equalsKeyPredicate)
			: this(getNameFunction,
				getterFunction,
				setterAction,
				equalsKeyPredicate,
				(origin, target) => origin.Equals(target))
		{
		}

		public DictionaryObjectInfo(Func<string> getNameFunction,
			Func<TInstance, IDictionary<TKey, TValue>> getterFunction,
			Action<TInstance, IDictionary<TKey, TValue>> setterAction,
			EqualsPredicate<TKey> equalsKeyPredicate,
			EqualsPredicate<TValue> itemEqualsPredicate)
		{
			_getNameFunction = getNameFunction;
			_getterFunction = getterFunction;
			_setterAction = setterAction;
			_equalsKeyPredicate = equalsKeyPredicate;
			_equalsValuePredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

		public void SetValueEqualsPredicate(EqualsPredicate<TValue> itemEqualsPredicate)
		{
			_equalsValuePredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}
		public void SetKeyEqualsPredicate(EqualsPredicate<TKey> equalsPredicate)
		{
			_equalsKeyPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		public string GetName()
		{
			return _getNameFunction();
		}

		public IDictionary<TKey, TValue> Get(TInstance instance)
		{
			return _getterFunction(instance);
		}

		public void Set(TInstance originalInstance, IDictionary<TKey, TValue> value)
		{
			_setterAction(originalInstance, value);
		}

		public bool IsKeyEquals(TKey originalValue, TKey targetValue)
		{
			return _equalsKeyPredicate(originalValue, targetValue);
		}

		public bool IsValueEquals(TValue originalValue, TValue targetValue)
		{
			return _equalsValuePredicate(originalValue, targetValue);
		}
	}
}