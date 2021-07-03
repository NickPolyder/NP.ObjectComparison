using System;

namespace ObjectComparison
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

		public ObjectInfo(Func<string> getNameFunction, Func<TInstance, TObject> getterFunction, Action<TInstance, TObject> setterAction)
			: this(getNameFunction, getterFunction, setterAction, (origin, target) => origin.Equals(target))
		{ }

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

		public void SetEqualsPredicate(EqualsPredicate<TObject> equalsPredicate)
		{
			_equalsPredicate = equalsPredicate ?? throw new ArgumentNullException(nameof(equalsPredicate));
		}

		public string GetName()
		{
			return _getNameFunction();
		}

		public TObject Get(TInstance instance)
		{
			return _getterFunction(instance);
		}

		public void Set(TInstance originalInstance, TObject targetValue)
		{
			_setterAction(originalInstance, targetValue);
		}

		public bool IsEqual(TObject originalValue, TObject targetValue)
		{
			return _equalsPredicate(originalValue, targetValue);
		}
		
	}
}