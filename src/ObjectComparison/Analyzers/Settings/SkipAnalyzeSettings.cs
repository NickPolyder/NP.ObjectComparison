using System;
using System.Collections.Concurrent;
using System.Reflection;
using ObjectComparison.Attributes;

namespace ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Skip Analyze Settings.
	/// </summary>
	public class SkipAnalyzeSettings
	{
		private readonly ConcurrentDictionary<Type, bool> _skippedTypes = new ConcurrentDictionary<Type, bool>();
		private readonly ConcurrentDictionary<PropertyInfo, bool> _skippedProperties = new ConcurrentDictionary<PropertyInfo, bool>();

		/// <summary>
		/// Is this <paramref name="type"/> skipped ?
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool IsSkipped(Type type)
		{
			return _skippedTypes.GetOrAdd(type, t => t.GetCustomAttribute<SkipAnalyzeAttribute>() != null);
		}

		/// <summary>
		/// Is this <paramref name="propertyInfo"/> skipped ?
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public bool IsSkipped(PropertyInfo propertyInfo)
		{
			return _skippedProperties.GetOrAdd(propertyInfo, t => t.GetCustomAttribute<SkipAnalyzeAttribute>() != null);
		}

		/// <summary>
		/// Clears the skipped types and properties list.
		/// </summary>
		public void Clear()
		{
			_skippedTypes.Clear();
			_skippedProperties.Clear();
		}

		/// <summary>
		/// Skips this <paramref name="type" />.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public SkipAnalyzeSettings Skip(Type type)
		{
			_skippedTypes.AddOrUpdate(type, true, (_, __) => true);
			return this;
		}

		/// <summary>
		///  Skips this <paramref name="propertyInfo" />.
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public SkipAnalyzeSettings Skip(PropertyInfo propertyInfo)
		{
			_skippedProperties.AddOrUpdate(propertyInfo, true, (_, __) => true);
			return this;
		}
	}
}