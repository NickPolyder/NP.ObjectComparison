using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NP.ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Extensions related to <see cref="SkipAnalyzeSettings"/>.
	/// </summary>
	public static class SkipAnalyzeSettingsExtensions
	{
		/// <summary>
		/// Skips the property given by the <paramref name="expression"/>.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="expression">A <see cref="MemberExpression"/> accessing a property.</param>
		/// <returns>Self.</returns>
		public static SkipAnalyzeSettings Skip<TModel, TMember>(this SkipAnalyzeSettings settings, Expression<Func<TModel, TMember>> expression)
		{
			if (expression?.Body is MemberExpression memberExpression 
			    && memberExpression.Member is PropertyInfo propertyInfo)
			{
				return settings.Skip(propertyInfo);
			}

			throw new ArgumentException(Resources.Errors.Skip_RequiresMemberExpression, nameof(expression));
		}


		/// <summary>
		/// Skips this <typeparamref name="TModel"/>.
		/// </summary>
		/// <param name="settings"></param>
		/// <returns>Self.</returns>
		public static SkipAnalyzeSettings Skip<TModel>(this SkipAnalyzeSettings settings)
		{
			return settings.Skip(typeof(TModel));
		}
	}
}