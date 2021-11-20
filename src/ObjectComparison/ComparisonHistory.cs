using System;
using System.Collections.Generic;
using ObjectComparison.Results;

namespace ObjectComparison
{
	/// <summary>
	/// A Concrete Comparison History.
	/// </summary>
	/// <typeparam name="TModel">The object type of the Comparison.</typeparam>
	public class ComparisonHistory<TModel> : IComparisonHistory<TModel>
	{
		private readonly TModel _current;
		private readonly IEnumerable<DiffSnapshot> _diffSnapshots;

		/// <summary>
		/// Creates a comparison history object.
		/// </summary>
		/// <param name="current">The current history object.</param>
		/// <param name="diffSnapshots">The current diff analysis.</param>
		public ComparisonHistory(TModel current, IEnumerable<DiffSnapshot> diffSnapshots)
		{
			_current = current;
			_diffSnapshots = diffSnapshots;
			ModifiedOn = DateTimeOffset.UtcNow;
		}

		/// <inheritdoc />
		public DateTimeOffset ModifiedOn { get; }

		/// <inheritdoc />
		public TModel Get()
		{
			return _current;
		}

		/// <inheritdoc />
		public IEnumerable<DiffSnapshot> GetDiff()
		{
			return _diffSnapshots;
		}
	}
}