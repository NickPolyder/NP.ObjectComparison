using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Results
{
	public static class ObjectItemExtensions
	{
		public static bool HasChanges(this IEnumerable<ObjectItem> enumerable)
		{
			return enumerable?.Any(item => item.HasChanges) ?? false;
		}
	}
}