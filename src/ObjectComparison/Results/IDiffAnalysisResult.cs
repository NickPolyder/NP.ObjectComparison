using System.Collections.Generic;

namespace ObjectComparison.Results
{
	public interface IDiffAnalysisResult: IEnumerable<DiffSnapshot>
	{
		bool IsPatched { get; }
		void Add(DiffSnapshot snapshot);

		void AddRange(IEnumerable<DiffSnapshot> snapshots);

		void Patch();

		void Merge(IDiffAnalysisResult otherResult);
	}
}