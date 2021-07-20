using System.Collections.Generic;
using System.Reflection;

namespace ObjectComparison
{
	/// <summary>
	/// Options associated with <see cref="Analyzers.AnalyzerBuilder{T}"/>
	/// to monitor object depth.
	/// </summary>
	public class TypeGeneratorBuilderOptions
	{
		public Dictionary<PropertyInfo, int> Depth = new Dictionary<PropertyInfo, int>();

		public int MaxDepth { get; set; } = Constants.MaxReferenceLoopDepth;

		public bool IsAllowedDepth(PropertyInfo propertyInfo)
		{
			var depth = Depth.GetOrAdd(propertyInfo, key => 0);
			
			return depth >= MaxDepth;
		}
	}
}