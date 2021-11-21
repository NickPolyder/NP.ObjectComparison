using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectComparison.Analyzers.Infos
{
	/// <summary>
	/// DictionaryObjectInfo Builder.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	public class DictionaryObjectInfoBuilder<TInstance>
	{
		/// <returns>A built <see cref="DictionaryObjectInfo{TInstance, TKey, TValue}"/>.</returns>
		public static object Build(PropertyInfo publicProperty)
		{
			var genericParameters = publicProperty.PropertyType.GetGenericArguments();

			var keyType = genericParameters[0];

			var valueType = genericParameters[1];

			var interfaceType = typeof(IDictionary<,>).MakeGenericType(keyType, valueType);

			var instanceParam = Expression.Parameter(typeof(TInstance), "instance");
			
			var propertyExpression = Expression.Property(instanceParam, publicProperty);
			
			var castPropertyExpression = Expression.Convert(propertyExpression, interfaceType);
			
			var getterLambdaExpression = Expression.Lambda(castPropertyExpression, instanceParam);

			var getterFunc = getterLambdaExpression.Compile();

			var targetParam = Expression.Parameter(interfaceType, "target");

			var castSetterExpression = Expression.Convert(targetParam, publicProperty.PropertyType);
			var assignExpression = Expression.Assign(propertyExpression, castSetterExpression);

			var actionType = typeof(Action<,>).MakeGenericType(typeof(TInstance), interfaceType);

			var setterLambdaExpression = Expression.Lambda(actionType, assignExpression, instanceParam, targetParam);

			var setterFunc = setterLambdaExpression.Compile();
			
			var objectInfoType = typeof(DictionaryObjectInfo<,,>)
				.MakeGenericType(typeof(TInstance), keyType, valueType)
				.GetConstructors()[0];

			var objectInfo = objectInfoType.Invoke(new object[]
			{
				new Func<string>(() => publicProperty.Name),
				getterFunc,
				setterFunc
			});

			return objectInfo;
		}
	}
}