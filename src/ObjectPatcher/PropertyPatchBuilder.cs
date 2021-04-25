using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ObjectPatcher
{
	public class PropertyPatchBuilder<TInstance>
	{

		public IEnumerable<IPatchInfo<TInstance>> Build()
		{
			var publicProperties = typeof(TInstance).GetProperties();

			foreach (var publicProperty in publicProperties)
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

				var propertyPatchType =
					typeof(PropertyPatch<,>)
						.MakeGenericType(typeof(TInstance), publicProperty.PropertyType)
						.GetConstructors()[0];

				yield return (IPatchInfo<TInstance>)propertyPatchType.Invoke(new object[]
				{
					new Func<string>(() => publicProperty.Name),
					getterFunc,
					setterFunc
				});
			}
		}
	}
}