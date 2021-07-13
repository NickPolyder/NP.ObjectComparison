using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Analyzers
{
	public static class ObjectAnalyzerExtensions
	{
		public static IObjectAnalyzer<TInstance> ToComposite<TInstance>(
			this IEnumerable<IObjectAnalyzer<TInstance>> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException(nameof(enumerable));
			}

			return new AnalyzerComposite<TInstance>(enumerable.ToArray());
		}
	}
}