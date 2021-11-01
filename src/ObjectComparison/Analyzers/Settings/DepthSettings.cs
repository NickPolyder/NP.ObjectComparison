using System.Collections.Generic;
using System.Reflection;

namespace ObjectComparison.Analyzers.Settings
{
	/// <summary>
	/// Depth settings
	/// </summary>
	public class DepthSettings
	{
		private readonly Dictionary<PropertyInfo, int> _depth = new Dictionary<PropertyInfo, int>();
		
		/// <summary>
		/// Max allowed depth.
		/// </summary>
		public int MaxDepth { get; set; } = Constants.MaxReferenceLoopDepth;

		/// <summary>
		/// Increase Depth by one.
		/// </summary>
		/// <param name="propertyInfo"></param>
		public void IncreaseDepth(PropertyInfo propertyInfo)
		{
			_depth[propertyInfo]++;
		}

		/// <summary>
		/// Is current depth allowed ?
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public bool IsAllowedDepth(PropertyInfo propertyInfo)
		{
			var depth = _depth.GetOrAdd(propertyInfo, key => 0);
			
			return depth >= MaxDepth;
		}
	}
}