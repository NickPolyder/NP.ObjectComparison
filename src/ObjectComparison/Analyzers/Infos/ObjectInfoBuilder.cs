using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectComparison.Analyzers.Infos
{
	/// <summary>
	/// ObjectInfo Builder.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	public class ObjectInfoBuilder<TInstance>
	{
		/// <returns>A built <see cref="ObjectInfo{TInstance, TObject}"/>.</returns>
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

			var objectInfoType = typeof(ObjectInfo<,>)
				.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
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