using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison
{
	public interface IComparisonTracker<TObject>
	{
		TObject Original { get; }

		TObject Current { get; set; }

		IEnumerable<DiffSnapshot> GetCurrentAnalysis();

		void Analyze();

		bool HasChanges();

		bool IsPatched();

		void Patch();

		void Reset();
	}
}