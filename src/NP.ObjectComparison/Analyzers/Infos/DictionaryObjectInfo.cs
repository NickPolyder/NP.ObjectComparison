using System;
using System.Collections.Generic;

namespace NP.ObjectComparison.Analyzers.Infos
{
	/// <summary>
	///  Wrapping object around the Patching functionality.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class DictionaryObjectInfo<TInstance, TKey, TValue>
	{
		private readonly Func<string> _getNameFunction;
		private readonly Func<TInstance, IDictionary<TKey, TValue>> _getterFunction;
		private readonly Action<TInstance, IDictionary<TKey, TValue>> _setterAction;
		private EqualsPredicate<TKey> _equalsKeyPredicate;
		private EqualsPredicate<TValue> _equalsValuePredicate;

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		public DictionaryObjectInfo(Func<string> getNameFunction,
			Func<TInstance, IDictionary<TKey, TValue>> getterFunction,
			Action<TInstance, IDictionary<TKey, TValue>> setterAction)
			: this(getNameFunction, getterFunction, setterAction,
				(origin, target) => object.Equals(origin, target),
				(origin, target) => object.Equals(origin, target))
		{ }

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		/// <param name="equalsKeyPredicate"></param>
		public DictionaryObjectInfo(Func<string> getNameFunction,
			Func<TInstance, IDictionary<TKey, TValue>> getterFunction,
			Action<TInstance, IDictionary<TKey, TValue>> setterAction,
			EqualsPredicate<TKey> equalsKeyPredicate)
			: this(getNameFunction,
				getterFunction,
				setterAction,
				equalsKeyPredicate,
				(origin, target) => object.Equals(origin, target))
		{
		}

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		/// <param name="equalsKeyPredicate"></param>
		/// <param name="itemEqualsPredicate"></param>
		/// <exception cref="ArgumentNullException"></exception>
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

		/// <summary>
		/// Sets the <typeparamref name="TValue"/> Equality delegate.
		/// </summary>
		/// <param name="itemEqualsPredicate"></param>
		/// <exception cref="ArgumentNullException"> When <paramref name="itemEqualsPredicate"/> is null.</exception>
		public void SetValueEqualsPredicate(EqualsPredicate<TValue> itemEqualsPredicate)
		{
			_equalsValuePredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

		/// <summary>
		/// Sets the <typeparamref name="TKey"/> Equality delegate.
		/// </summary>
		/// <param name="equalsPredicate"></param>
		/// <exception cref="ArgumentNullException"> When <paramref name="equalsPredicate"/> is null.</exception>
		public void SetKeyEqualsPredicate(EqualsPredicate<TKey> equalsPredicate)
		{
			_equalsKeyPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		/// <summary>
		/// Gets the name of the object.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return _getNameFunction();
		}

		/// <summary>
		/// Gets the object for this <paramref name="instance"/>.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public IDictionary<TKey, TValue> Get(TInstance instance)
		{
			return _getterFunction(instance);
		}

		/// <summary>
		/// Sets <paramref name="value"/> to <paramref name="originalInstance"/>.
		/// </summary>
		/// <param name="originalInstance"></param>
		/// <param name="value"></param>
		public void Set(TInstance originalInstance, IDictionary<TKey, TValue> value)
		{
			_setterAction(originalInstance, value);
		}

		/// <summary>
		/// Is the dictionary key <paramref name="originalValue"/> equal to <paramref name="targetValue"/>.
		/// </summary>
		/// <param name="originalValue"></param>
		/// <param name="targetValue"></param>
		/// <returns></returns>
		public bool IsKeyEquals(TKey originalValue, TKey targetValue)
		{
			return _equalsKeyPredicate(originalValue, targetValue);
		}

		/// <summary>
		/// Is the dictionary value <paramref name="originalValue"/> equal to <paramref name="targetValue"/>.
		/// </summary>
		/// <param name="originalValue"></param>
		/// <param name="targetValue"></param>
		/// <returns></returns>
		public bool IsValueEquals(TValue originalValue, TValue targetValue)
		{
			return _equalsValuePredicate(originalValue, targetValue);
		}
	}
}