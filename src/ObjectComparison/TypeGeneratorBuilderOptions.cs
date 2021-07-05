using System.Collections.Generic;
using System.Reflection;

namespace ObjectComparison
{
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