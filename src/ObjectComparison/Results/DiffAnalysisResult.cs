using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Results
{
	/// <summary>
	/// The Difference Analysis Result.
	/// </summary>
	public class DiffAnalysisResult: IDiffAnalysisResult
	{
		private readonly List<DiffSnapshot> _diffSnapshots;
		private Action _patchAction;

		/// <inheritdoc />
		public bool IsPatched { get; private set; }

		/// <summary>
		/// Constructs this object.
		/// </summary>
		public DiffAnalysisResult(): this(Enumerable.Empty<DiffSnapshot>())
		{ }

		/// <summary>
		/// Constructs this object with <paramref name="diffSnapshots"/>.
		/// </summary>
		/// <param name="diffSnapshots"></param>
		public DiffAnalysisResult(IEnumerable<DiffSnapshot> diffSnapshots):this(diffSnapshots, null)
		{ }

		/// <summary>
		/// Constructs this object with <paramref name="diffSnapshots"/> and <paramref name="patchAction"/>.
		/// </summary>
		/// <param name="diffSnapshots"></param>
		/// <param name="patchAction"></param>
		public DiffAnalysisResult(IEnumerable<DiffSnapshot> diffSnapshots, Action patchAction)
		{
			_diffSnapshots = diffSnapshots?.ToList() ?? new List<DiffSnapshot>();
			_patchAction = patchAction;
		}

		/// <inheritdoc />
		public void AddPatchAction(Action patchAction)
		{
			_patchAction = patchAction ?? throw new ArgumentNullException(nameof(patchAction));
		}

		/// <inheritdoc />
		public void MergePatchAction(Action patchAction)
		{
			var oldPatchAction = _patchAction;
			_patchAction = () =>
			{
				oldPatchAction?.Invoke();
				patchAction?.Invoke();
			};
		}

		/// <inheritdoc />
		public void Add(DiffSnapshot snapshot)
		{
			_diffSnapshots.Add(snapshot);
		}

		/// <inheritdoc />
		public void AddRange(IEnumerable<DiffSnapshot> snapshots)
		{
			_diffSnapshots.AddRange(snapshots);
		}

		/// <inheritdoc />
		public void Patch()
		{
			_patchAction?.Invoke();
			IsPatched = true;
		}

		/// <inheritdoc />
		public void Merge(IDiffAnalysisResult otherResult)
		{
			_diffSnapshots.AddRange(otherResult);

			MergePatchAction(new Action(otherResult.Patch));
		}

		/// <inheritdoc />
		public IEnumerator<DiffSnapshot> GetEnumerator()
		{
			return _diffSnapshots.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}