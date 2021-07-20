using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison
{
	/// <summary>
	/// An object that tracks changes for a specific instance of <typeparamref name="TObject"/>
	/// </summary>
	/// <typeparam name="TObject">The type that needs to be tracked.</typeparam>
	public interface IComparisonTracker<TObject>
	{
		/// <summary>
		/// The original instance.
		/// </summary>
		TObject Original { get; }

		/// <summary>
		/// The current instance.
		/// </summary>
		TObject Current { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerable<DiffSnapshot> GetCurrentAnalysis();

		void Analyze();

		bool HasChanges(bool autoAnalyze = false);

		bool IsPatched();

		void Patch();

		void Reset();
	}
}