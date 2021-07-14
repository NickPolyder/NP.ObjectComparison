using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison
{
	public static class TypeExtensions
	{
		private static Type _IEnumerable = typeof(IEnumerable);
		private static Type _GenericIEnumerable = typeof(IEnumerable<>);
		public static bool IsCollectionType(this Type type)
		{
			return type != typeof(string)
			       && (_GenericIEnumerable.IsAssignableFrom(type)
			           || _IEnumerable.IsAssignableFrom(type));
		}

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

		public static bool HasInterface<TInterfaceType>(this Type @thisType)
		{
			return @thisType.HasInterface(typeof(TInterfaceType));
		}

		public static bool HasInterface(this Type @thisType, Type interfaceType)
		{

			return interfaceType.IsAssignableFrom(@thisType) || 
			       @thisType.GetInterfaces().Any(item => item == interfaceType 
			                                                          || (item.IsGenericType 
			                                                              && item.GetGenericTypeDefinition() == interfaceType));
		}
	}
}