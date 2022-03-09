using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NP.ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Extensions related to <see cref="IgnoreSettings"/>.
	/// </summary>
	public static class IgnoreAnalyzeSettingsExtensions
	{
		/// <summary>
		/// Ignores the property given by the <paramref name="expression"/>.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="expression">A <see cref="MemberExpression"/> accessing a property.</param>
		/// <returns>Self.</returns>
		public static IgnoreSettings Ignore<TModel, TMember>(this IgnoreSettings settings, Expression<Func<TModel, TMember>> expression)
		{
			if (expression?.Body is MemberExpression memberExpression 
			    && memberExpression.Member is PropertyInfo propertyInfo)
			{
				return settings.Ignore(propertyInfo);
			}

			throw new ArgumentException(Resources.Errors.Skip_RequiresMemberExpression, nameof(expression));
		}


		/// <summary>
		/// Ignores this <typeparamref name="TModel"/>.
		/// </summary>
		/// <param name="settings"></param>
		/// <returns>Self.</returns>
		public static IgnoreSettings Ignore<TModel>(this IgnoreSettings settings)
		{
			return settings.Ignore(typeof(TModel));
		}
	}
}