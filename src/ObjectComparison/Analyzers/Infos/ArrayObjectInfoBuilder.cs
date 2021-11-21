using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectComparison.Analyzers.Infos
{
	/// <summary>
	/// ArrayObjectInfoBuilder Builder.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	public class ArrayObjectInfoBuilder<TInstance>
	{
		/// <returns>A built <see cref="ArrayObjectInfo{TInstance, TArray, TArrayOf}"/>.</returns>
		public static object Build(PropertyInfo publicProperty)
		{
			var instanceParam = Expression.Parameter(typeof(TInstance), "instance");
			var propertyExpression = Expression.Property(instanceParam, publicProperty);

			var getterLambdaExpression = Expression.Lambda(propertyExpression, instanceParam);

			var getterFunc = getterLambdaExpression.Compile();

			var targetParam = Expression.Parameter(publicProperty.PropertyType, "target");

			var assignExpression = Expression.Assign(propertyExpression, targetParam);

			var actionType = typeof(Action<,>).MakeGenericType(typeof(TInstance), publicProperty.PropertyType);

			var setterLambdaExpression = Expression.Lambda(actionType, assignExpression, instanceParam, targetParam);

			var setterFunc = setterLambdaExpression.Compile();

			var arrayOf = publicProperty.PropertyType.GetCollectionElementType();

			var objectInfoType = typeof(ArrayObjectInfo<,,>)
				.MakeGenericType(typeof(TInstance), publicProperty.PropertyType, arrayOf)
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