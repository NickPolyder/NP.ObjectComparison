using System;
using System.Collections.Generic;
using System.Linq;
using NP.ObjectComparison.Analyzers;
using NP.ObjectComparison.Exceptions;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison
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
		private List<IComparisonHistory<TObject>> _history;

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
					CloneToOriginal();
				}
			}
		}

		/// <summary>
		/// Constructs a Comparison Tracker with <paramref name="currentValue"/> and optionally <paramref name="cloneFunc"/>.
		/// </summary>
		/// <param name="currentValue">The current value to monitor.</param>
		/// <param name="cloneFunc">The function to clone the <typeparamref name="TObject"/>.</param>
		public ComparisonTracker(TObject currentValue, Func<TObject, TObject> cloneFunc = null) : this(currentValue,
			AnalyzerBuilder<TObject>.Build().ToComposite(), cloneFunc)
		{ }

		/// <summary>
		/// Constructs a Comparison Tracker with <paramref name="currentValue"/> and optionally <paramref name="cloneFunc"/>.
		/// </summary>
		/// <param name="currentValue">The current value to monitor.</param>
		/// <param name="analyzer">The Analyzer.</param>
		/// <param name="cloneFunc">The function to clone the <typeparamref name="TObject"/>.</param>
		public ComparisonTracker(TObject currentValue, IObjectAnalyzer<TObject> analyzer, Func<TObject, TObject> cloneFunc = null)
		{
			_current = currentValue;
			_analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
			_history = new List<IComparisonHistory<TObject>>();
			_cloneFunc = cloneFunc;
			CloneToOriginal();
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
		public IEnumerable<IComparisonHistory<TObject>> GetHistory()
		{			
			return _history.AsEnumerable();
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

			_history.Add(new ComparisonHistory<TObject>(CloneValue(), _currentAnalysis));
		}

		/// <inheritdoc />
		public void Reset()
		{
			Original = CloneValue();
			_currentAnalysis = null;
		}

		/// <inheritdoc />
		public void RevertTo(IComparisonHistory<TObject> history)
		{
			Current = history.Get();
			Analyze();
		}

		private void CloneToOriginal()
		{
			_history.Add(new ComparisonHistory<TObject>(CloneValue(), Enumerable.Empty<DiffSnapshot>()));
			Original = CloneValue();
		}

		private TObject CloneValue()
		{
			if (_cloneFunc != null)
			{
				return _cloneFunc.Invoke(Current);
			}

			if (Current == null)
			{
				return default;
			}

			if (Current is ICloneable cloneable)
			{
				return (TObject)cloneable.Clone();
			}

			throw new ObjectComparisonException(Resources.Errors.CannotClone);
		}

		/// <summary>
		/// Casts to <typeparamref name="TObject" />.
		/// </summary>
		/// <param name="tracker"></param>
		/// <returns></returns>
		public static implicit operator TObject(ComparisonTracker<TObject> tracker) => tracker == null ? default : tracker.Current;

		/// <summary>
		/// Casts from <typeparamref name="TObject" />.
		/// </summary>
		/// <param name="currentValue"></param>
		/// <returns></returns>
		public static implicit operator ComparisonTracker<TObject>(TObject currentValue)
		{
			return new ComparisonTracker<TObject>(currentValue);
		}
	}
}