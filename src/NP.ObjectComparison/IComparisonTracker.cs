using System.Collections.Generic;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison
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

		/// <returns>
		/// A collection of the difference results from the active analysis.
		/// </returns>
		IEnumerable<DiffSnapshot> GetCurrentAnalysis();

		/// <summary>
		/// Gets the history list.
		/// </summary>
		/// <returns>A history object.</returns>
		IEnumerable<IComparisonHistory<TObject>> GetHistory();

		/// <summary>
		/// Analyzes the <see cref="Original"/> and <see cref="Current"/> and generates
		/// a report of the comparison of the two objects.
		/// <para>
		/// The report can be retrieved by calling <seealso cref="GetCurrentAnalysis"/>
		/// </para>
		/// </summary>
		void Analyze();

		/// <summary>
		/// Has the <see cref="Current"/> been changed.
		/// </summary>
		/// <param name="autoAnalyze">When true it will call <see cref="Analyze"/> before it analyzes the objects.</param>
		/// <returns>True when the objects have differences, false otherwise.</returns>
		bool HasChanges(bool autoAnalyze = false);

		/// <summary>
		/// Has the <see cref="Original"/> been patched with <see cref="Current"/> ?
		/// </summary>
		/// <returns>True when the object has been patched.</returns>
		bool IsPatched();

		/// <summary>
		/// Patches the <see cref="Original"/> with the <see cref="Current"/>.
		/// </summary>
		void Patch();

		/// <summary>
		/// Resets the values based on <paramref name="toCurrent"/>.
		/// </summary>
		/// <param name="toCurrent">When true <see cref="Current"/> is copied to <seealso cref="Original"/>.
		/// Otherwise <see cref="Original"/> is copied to <seealso cref="Current"/>.</param>
		void Reset(bool toCurrent = false);

		/// <summary>
		/// Reverts this Tracker to a previous state encapsulated in <paramref name="history"/>.
		/// </summary>
		/// <param name="history">The history object.</param>
		void RevertTo(IComparisonHistory<TObject> history);
	}
}