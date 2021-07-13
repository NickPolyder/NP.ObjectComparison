using System;
using ObjectComparison.Analyzers;
using ObjectComparison.Exceptions;
using ObjectComparison.Results;

namespace ObjectComparison
{
	public class ComparisonTracker<TObject>: IComparisonTracker<TObject>
	{
		private readonly Func<TObject, TObject> _cloneFunc;
		private readonly IObjectAnalyzer<TObject> _analyzer;
		private TObject _original;
		private TObject _current;
		private bool _isOriginalSet = false;
		private IDiffAnalysisResult _currentAnalysis;

		public TObject Original
		{
			get => _original;
			private set
			{
				_original = value;
				_isOriginalSet = true;
			}
		}

		public TObject Current
		{
			get => _current;
			set
			{
				_current = value;

				if (!_isOriginalSet)
				{
					CloneValue();
				}
			}
		}

		public ComparisonTracker(TObject currentValue, Func<TObject, TObject> cloneFunc = null) : this(currentValue,
			AnalyzerBuilder<TObject>.Build().ToComposite(), cloneFunc)
		{ }

		public ComparisonTracker(TObject currentValue, IObjectAnalyzer<TObject> analyzer, Func<TObject, TObject> cloneFunc = null)
		{
			Current = currentValue;
			_analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
			_cloneFunc = cloneFunc;
			CloneValue();
		}

		public void Analyze()
		{
			_currentAnalysis = _analyzer.Analyze(Original, Current);
		}

		public bool HasChanges()
		{
			if (_currentAnalysis == null)
			{
				Analyze();
			}

			return _currentAnalysis.HasChanges();
		}

		public bool IsPatched()
		{
			return _currentAnalysis != null && _currentAnalysis.IsPatched;
		}

		public void Patch()
		{
			if (_currentAnalysis == null)
			{
				Analyze();
			}

			if (!_currentAnalysis.IsPatched)
			{
				_currentAnalysis.Patch();
			}
		}
		
		public void Reset()
		{
			CloneValue();
			_currentAnalysis = null;
		}

		private void CloneValue()
		{
			if (_cloneFunc != null)
			{
				Original = _cloneFunc.Invoke(Current);
				return;
			}

			if (Current == null)
			{
				Original = default;
				return;
			}

			if (Current is ICloneable cloneable)
			{
				Original = (TObject)cloneable.Clone();
				return;
			}

			throw new ObjectComparisonException(Resources.Errors.CannotClone);
		}

		public static implicit operator TObject(ComparisonTracker<TObject> tracker) => tracker == null ? default : tracker.Current;

		public static implicit operator ComparisonTracker<TObject>(TObject currentValue)
		{
			return new ComparisonTracker<TObject>(currentValue);
		}
	}
}