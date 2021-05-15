using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPatcher
{
	public class ArrayObjectInfo<TInstance, TArray, TArrayOf>: ObjectInfo<TInstance, TArray>
	{
		private EqualsPredicate<TArrayOf> _itemEqualsPredicate;

		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction)
			: this(getNameFunction, getterFunction, setterAction, 
				(origin, target) => origin.Equals(target),
				(origin, target) => origin.Equals(target))
		{ }

		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction,
			EqualsPredicate<TArray> equalsPredicate): base(getNameFunction, getterFunction, setterAction, equalsPredicate)
		{
		}
		
		public ArrayObjectInfo(Func<string> getNameFunction,
			Func<TInstance, TArray> getterFunction,
			Action<TInstance, TArray> setterAction,
			EqualsPredicate<TArray> equalsPredicate,
			EqualsPredicate<TArrayOf> itemEqualsPredicate) : this(getNameFunction, getterFunction, setterAction, equalsPredicate)
		{
			_itemEqualsPredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

		public void SetItemEqualsPredicate(EqualsPredicate<TArrayOf> itemEqualsPredicate)
		{
			_itemEqualsPredicate = itemEqualsPredicate ?? throw new ArgumentNullException(nameof(itemEqualsPredicate));
		}

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

		public bool IsItemEqual(TArrayOf originalValue, TArrayOf targetValue)
		{
			return _itemEqualsPredicate(originalValue, targetValue);
		}
	}
}