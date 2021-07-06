using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Results
{
	public static class DiffSnapshotExtensions
	{
		public static bool HasChanges(this IEnumerable<DiffSnapshot> enumerable)
		{
			return enumerable?.Any(item => item.HasChanges) ?? false;
		}
	}
}