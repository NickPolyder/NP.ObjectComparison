using System;
using System.Collections.Generic;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison
{
	/// <summary>
	/// The Comparison History
	/// </summary>
	/// <typeparam name="TModel">The object type of the Comparison.</typeparam>
	public interface IComparisonHistory<TModel>
	{

		/// <summary>
		/// The Date and time that this history item was created.
		/// </summary>
		DateTimeOffset ModifiedOn { get; }

		/// <returns>
		/// The data of this history object.
		/// </returns>
		TModel Get();

		/// <returns>
		/// The Diff result of this history object.
		/// </returns>
		IEnumerable<DiffSnapshot> GetDiff();
	}
}