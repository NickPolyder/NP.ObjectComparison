using System.Collections.Generic;
using System.Linq;

namespace ObjectPatcher.Results
{
	public static class PatchItemExtensions
	{
		public static bool HasChanges(this IEnumerable<PatchItem> enumerable)
		{
			return enumerable?.Any(item => item.HasChanges) ?? false;
		}
	}
}