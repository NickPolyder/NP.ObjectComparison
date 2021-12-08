using System.Collections.Generic;

namespace NP.ObjectComparison.Results
{
	/// <summary>
	/// The Difference Analysis result.
	/// </summary>
	public interface IDiffAnalysisResult: IEnumerable<DiffSnapshot>
	{
		/// <summary>
		/// Has these Differences being already applied ?
		/// </summary>
		bool IsPatched { get; }

		/// <summary>
		/// Add a <paramref name="snapshot"/> to the Difference Analysis Result.
		/// </summary>
		/// <param name="snapshot"></param>
		void Add(DiffSnapshot snapshot);

		/// <summary>
		/// Adds a range of <paramref name="snapshots"/> to the Difference Analysis Result.
		/// </summary>
		/// <param name="snapshots"></param>
		void AddRange(IEnumerable<DiffSnapshot> snapshots);

		/// <summary>
		/// Apply the patch of this Difference Analysis Result.
		/// </summary>
		void Patch();

		/// <summary>
		/// Merges <paramref name="otherResult"/> to this Difference Analysis Result.
		/// </summary>
		/// <param name="otherResult"></param>
		void Merge(IDiffAnalysisResult otherResult);
	}
}