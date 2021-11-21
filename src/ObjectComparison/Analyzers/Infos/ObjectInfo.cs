using System;

namespace ObjectComparison.Analyzers.Infos
{
	/// <summary>
	/// Wrapping object around the Patching functionality.
	/// </summary>
	public class ObjectInfo<TInstance, TObject>
	{
		private readonly Func<string> _getNameFunction;
		private readonly Func<TInstance, TObject> _getterFunction;
		private readonly Action<TInstance, TObject> _setterAction;
		private EqualsPredicate<TObject> _equalsPredicate;

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		public ObjectInfo(Func<string> getNameFunction, Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction)
			: this(getNameFunction, getterFunction, setterAction, (origin, target) => object.Equals(origin, target))
		{ }

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		/// <param name="equalsPredicate"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TObject> getterFunction,
			Action<TInstance, TObject> setterAction,
			EqualsPredicate<TObject> equalsPredicate)
		{
			_getNameFunction = getNameFunction ?? throw new ArgumentNullException(nameof(getNameFunction));
			_getterFunction = getterFunction ?? throw new ArgumentNullException(nameof(getterFunction));
			_setterAction = setterAction ?? throw new ArgumentNullException(nameof(setterAction));
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="equalsPredicate"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetEqualsPredicate(EqualsPredicate<TObject> equalsPredicate)
		{
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
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
		public TObject Get(TInstance instance)
		{
			return _getterFunction(instance);
		}

		/// <summary>
		/// Sets <paramref name="targetValue"/> to <paramref name="originalInstance"/>.
		/// </summary>
		/// <param name="originalInstance"></param>
		/// <param name="targetValue"></param>
		public void Set(TInstance originalInstance, TObject targetValue)
		{
			_setterAction(originalInstance, targetValue);
		}

		/// <summary>
		/// Is <paramref name="originalValue"/> equal to <paramref name="targetValue"/>.
		/// </summary>
		/// <param name="originalValue"></param>
		/// <param name="targetValue"></param>
		/// <returns></returns>
		public bool IsEqual(TObject originalValue, TObject targetValue)
		{
			return _equalsPredicate(originalValue, targetValue);
		}
		
	}
}