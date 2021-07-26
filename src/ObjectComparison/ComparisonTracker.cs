using System;
using System.Collections.Generic;
using ObjectComparison.Analyzers;
using ObjectComparison.Exceptions;
using ObjectComparison.Results;

namespace ObjectComparison
{
	/// <summary>
	/// An object that tracks changes for a specific instance of <typeparamref name="TObject"/>
	/// </summary>
	/// <typeparam name="TObject">The type that needs to be tracked.</typeparam>
	public class ComparisonTracker<TObject>: IComparisonTracker<TObject>
	{
		private readonly Func<TObject, TObject> _cloneFunc;
		private readonly IObjectAnalyzer<TObject> _analyzer;
		private TObject _original;
		private TObject _current;
		private bool _isOriginalSet = false;
		private IDiffAnalysisResult _currentAnalysis;

		/// <inheritdoc />
		public TObject Original
		{
			get => _original;
			private set
			{
				_original = value;
				_isOriginalSet = true;
			}
		}

		/// <inheritdoc />
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
			_current = currentValue;
			_analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
			_cloneFunc = cloneFunc;
			CloneValue();
		}

		/// <inheritdoc />
		public IEnumerable<DiffSnapshot> GetCurrentAnalysis()
		{
			if (_currentAnalysis == null)
			{
				Analyze();
			}

			return _currentAnalysis;
		}

		/// <inheritdoc />
		public void Analyze()
		{
			_currentAnalysis = _analyzer.Analyze(Original, Current);
		}

		/// <inheritdoc />
		public bool HasChanges(bool autoAnalyze = false)
		{
			if (autoAnalyze || _currentAnalysis == null)
			{
				Analyze();
			}

			return !IsPatched() && _currentAnalysis.HasChanges();
		}

		/// <inheritdoc />
		public bool IsPatched()
		{
			return _currentAnalysis != null && _currentAnalysis.IsPatched;
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
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