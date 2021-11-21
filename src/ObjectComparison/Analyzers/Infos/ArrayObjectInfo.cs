using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Analyzers.Infos
{
	/// <summary>
	/// Wrapping object around the Patching functionality.
	/// </summary>
	public class ArrayObjectInfo<TInstance, TArray, TArrayOf>: ObjectInfo<TInstance, TArray>
	{
		private EqualsPredicate<TArrayOf> _itemEqualsPredicate;

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction)
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
		/// <param name="equalsPredicate"></param>
		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction,
			EqualsPredicate<TArray> equalsPredicate): base(getNameFunction, getterFunction, setterAction, equalsPredicate)
		{
		}

		/// <summary>
		/// Construct this object.
		/// </summary>
		/// <param name="getNameFunction"></param>
		/// <param name="getterFunction"></param>
		/// <param name="setterAction"></param>
		/// <param name="equalsPredicate"></param>
		/// <param name="itemEqualsPredicate"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction,
			EqualsPredicate<TArray> equalsPredicate,
			EqualsPredicate<TArrayOf> itemEqualsPredicate) : this(getNameFunction, getterFunction, setterAction, equalsPredicate)
		{
			_itemEqualsPredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

		/// <summary>
		/// Sets the <typeparamref name="TArrayOf"/> Equality delegate.
		/// </summary>
		/// <param name="itemEqualsPredicate"></param>
		/// <exception cref="ArgumentNullException"> When <paramref name="itemEqualsPredicate"/> is null.</exception>
		public void SetItemEqualsPredicate(EqualsPredicate<TArrayOf> itemEqualsPredicate)
		{
			_itemEqualsPredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

		/// <summary>
		///  Gets the array object for this <paramref name="instance"/>.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public TArrayOf[] GetArray(TInstance instance)
		{
			var value = Get(instance);

			if (value is TArrayOf[] array)
			{
				return array;
			}else if (value is IEnumerable<TArrayOf> enumerable)
			{
				return enumerable.ToArray();
			}

			return (value as IEnumerable)?.Cast<TArrayOf>().ToArray() ?? Array.Empty<TArrayOf>();
		}

		/// <summary>
		/// Is <paramref name="originalValue"/> equal to <paramref name="targetValue"/>.
		/// </summary>
		/// <param name="originalValue"></param>
		/// <param name="targetValue"></param>
		/// <returns></returns>
		public bool IsItemEqual(TArrayOf originalValue, TArrayOf targetValue)
		{
			return _itemEqualsPredicate(originalValue, targetValue);
		}
	}
}