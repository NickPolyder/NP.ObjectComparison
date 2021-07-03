using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectComparison
{
	public class TypeGeneratorBuilderOptions
	{
		public Dictionary<PropertyInfo, int> Depth = new Dictionary<PropertyInfo, int>();

		public int MaxDepth { get; set; } = 5;
	}
}