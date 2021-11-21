using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Results
{
	/// <summary>
	/// Extensions for <see cref="DiffSnapshot"/>.
	/// </summary>
	public static class DiffSnapshotExtensions
	{
		/// <summary>
		/// Traverses the enumerable to find if any of the <see cref="DiffSnapshot"/> have changes.
		/// </summary>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		public static bool HasChanges(this IEnumerable<DiffSnapshot> enumerable)
		{
			return enumerable?.Any(item => item.HasChanges) ?? false;
		}
	}
}