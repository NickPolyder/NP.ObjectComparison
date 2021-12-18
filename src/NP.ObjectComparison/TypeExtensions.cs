using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NP.ObjectComparison
{
	/// <summary>
	/// Extensions for <see cref="Type"/>.
	/// </summary>
	public static class TypeExtensions
	{
		private static Type _IEnumerable = typeof(IEnumerable);
		private static Type _GenericIEnumerable = typeof(IEnumerable<>);

		/// <summary>
		/// Is the <paramref name="type"/> a collection type.
		/// </summary>
		/// <param name="type">The type to be checked.</param>
		/// <returns>True when the type is a collection type.</returns>
		public static bool IsCollectionType(this Type type)
		{
			return type != typeof(string)
				   && (_GenericIEnumerable.IsAssignableFrom(type)
					   || _IEnumerable.IsAssignableFrom(type));
		}

		/// <summary>
		/// Gets the element type of a generic collection or array.
		/// </summary>
		/// <param name="type">The collection type.</param>
		/// <returns>The element type of a generic collection or array.</returns>
		public static Type GetCollectionElementType(this Type type)
		{
			if (type.IsArray)
			{
				return type.GetElementType();
			}

			if (type.IsCollectionType())
			{
				if (type.IsGenericType)
				{
					var enumerableInterface = type.GetInterfaces()
						.First(interfaceType => interfaceType == _IEnumerable
												|| (interfaceType.IsGenericType
													&& interfaceType.GetGenericTypeDefinition() ==
													_GenericIEnumerable));
					if (enumerableInterface.IsGenericType)
					{
						return enumerableInterface.GetGenericArguments()[0];
					}
				}

				return typeof(object);
			}

			return null;
		}

		/// <summary>
		/// Does <paramref name="thisType"/> implement <typeparamref name="TInterfaceType"/>.
		/// </summary>
		/// <typeparam name="TInterfaceType">The interface type that has been inherited.</typeparam>
		/// <param name="thisType">The type that needs to implement <typeparamref name="TInterfaceType"/>.</param>
		/// <returns>True when <paramref name="thisType"/> implements <typeparamref name="TInterfaceType"/>.</returns>
		public static bool HasInterface<TInterfaceType>(this Type @thisType)
		{
			return @thisType.HasInterface(typeof(TInterfaceType));
		}

		/// <summary>
		/// Does <paramref name="thisType"/> implement <paramref name="interfaceType"/>.
		/// </summary>
		/// <param name="thisType">The type that needs to implement <paramref name="interfaceType"/>.</param>
		/// <param name="interfaceType">The interface type that has been inherited.</param>
		/// <returns>True when <paramref name="thisType"/> implements <paramref name="interfaceType"/>.</returns>
		public static bool HasInterface(this Type @thisType, Type interfaceType)
		{

			return interfaceType.IsAssignableFrom(@thisType) ||
				   @thisType.GetInterfaces().Any(item => item == interfaceType
														 || (item.IsGenericType
															 && item.GetGenericTypeDefinition() == interfaceType));
		}

		/// <summary>
		/// Is this type a <see cref="Nullable{T}"/> ?
		/// </summary>
		/// <param name="thisType"></param>
		/// <returns></returns>
		public static bool IsNullable(this Type @thisType)
		{
			return @thisType.IsGenericType
				&& @thisType.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
	}
}