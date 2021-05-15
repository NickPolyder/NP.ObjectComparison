﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPatcher
{
	public static class TypeExtensions
	{
		public static bool IsCollectionType(this Type type)
		{
			return type != typeof(string)
			       && (typeof(IEnumerable<>).IsAssignableFrom(type)
			           || typeof(IEnumerable).IsAssignableFrom(type));
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
					return type.GetInterfaces()
						.First(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
						.GetGenericArguments()[0];
				}

				return typeof(object);
			}

			return null;
		}
	}
}