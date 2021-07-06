using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Results
{
	public class DiffAnalysisResult: IDiffAnalysisResult
	{
		private readonly List<DiffSnapshot> _diffSnapshots;
		private Action _patchAction;
		public bool IsPatched { get; private set; }

		public DiffAnalysisResult(): this(Enumerable.Empty<DiffSnapshot>())
		{ }

		public DiffAnalysisResult(IEnumerable<DiffSnapshot> diffSnapshots):this(diffSnapshots, null)
		{ }

		public DiffAnalysisResult(IEnumerable<DiffSnapshot> diffSnapshots, Action patchAction)
		{
			_diffSnapshots = diffSnapshots?.ToList() ?? new List<DiffSnapshot>();
			_patchAction = patchAction;
		}

		public void AddPatchAction(Action patchAction)
		{
			_patchAction = patchAction ?? throw new ArgumentNullException(nameof(patchAction));
		}

		public void MergePatchAction(Action patchAction)
		{
			var oldPatchAction = _patchAction;
			_patchAction = () =>
			{
				oldPatchAction?.Invoke();
				patchAction?.Invoke();
			};
		}


		public void Add(DiffSnapshot snapshot)
		{
			_diffSnapshots.Add(snapshot);
		}

		public void AddRange(IEnumerable<DiffSnapshot> snapshots)
		{
			_diffSnapshots.AddRange(snapshots);
		}

		public void Patch()
		{
			_patchAction?.Invoke();
			IsPatched = true;
		}

		public void Merge(IDiffAnalysisResult otherResult)
		{
			_diffSnapshots.AddRange(otherResult);

			MergePatchAction(new Action(otherResult.Patch));
		}

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