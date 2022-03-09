using System;
using System.Collections.Concurrent;
using System.Reflection;
using NP.ObjectComparison.Attributes;

namespace NP.ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Ignore Settings for analyzers.
	/// </summary>
	public class IgnoreSettings
	{
		private readonly ConcurrentDictionary<Type, bool> _ignoredTypes = new ConcurrentDictionary<Type, bool>();
		private readonly ConcurrentDictionary<PropertyInfo, bool> _ignoredProperties = new ConcurrentDictionary<PropertyInfo, bool>();

		/// <summary>
		/// Is this <paramref name="type"/> ignored ?
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool IsIgnored(Type type)
		{
			return _ignoredTypes.GetOrAdd(type, t => t.GetCustomAttribute<IgnoreAttribute>() != null);
		}

		/// <summary>
		/// Is this <paramref name="propertyInfo"/> ignored ?
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public bool IsIgnored(PropertyInfo propertyInfo)
		{
			return _ignoredProperties.GetOrAdd(propertyInfo, t => t.GetCustomAttribute<IgnoreAttribute>() != null);
		}

		/// <summary>
		/// Clears the ignored types and properties list.
		/// </summary>
		public void Clear()
		{
			_ignoredTypes.Clear();
			_ignoredProperties.Clear();
		}

		/// <summary>
		/// Ignores this <paramref name="type" />.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IgnoreSettings Ignore(Type type)
		{
			_ignoredTypes.AddOrUpdate(type, true, (_, __) => true);
			return this;
		}

		/// <summary>
		///  Ignores this <paramref name="propertyInfo" />.
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public IgnoreSettings Ignore(PropertyInfo propertyInfo)
		{
			_ignoredProperties.AddOrUpdate(propertyInfo, true, (_, __) => true);
			return this;
		}
	}
}