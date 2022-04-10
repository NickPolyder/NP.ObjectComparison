using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NP.ObjectComparison.Analyzers;
using NP.ObjectComparison.Analyzers.Settings;
using NP.ObjectComparison.Exceptions;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison
{
	/// <summary>
	/// An object that tracks changes for a specific instance of <typeparamref name="TObject"/>
	/// </summary>
	/// <typeparam name="TObject">The type that needs to be tracked.</typeparam>
	public class ComparisonTracker<TObject> : IComparisonTracker<TObject>, IDisposable
	{
		private bool _isDisposed = false;
		private readonly Func<TObject, TObject> _cloneFunc;
		private readonly IObjectAnalyzer<TObject> _analyzer;
		private TObject _original;
		private TObject _current;
		private bool _isOriginalSet = false;
		private IDiffAnalysisResult _currentAnalysis;
		private readonly List<IComparisonHistory<TObject>> _history;

		/// <inheritdoc />
		public TObject Original
		{
			get
			{
				ThrowIfDisposed();
				return _original;
			}
			private set
			{
				_original = value;
				_isOriginalSet = true;
			}
		}

		/// <inheritdoc />
		public TObject Current
		{
			get
			{
				ThrowIfDisposed();
				return _current;
			}
			set
			{
				ThrowIfDisposed();
				UnRegisterNotifyPropertyChangedEvent();

				_current = value;

				if (!_isOriginalSet)
				{
					CloneToOriginal();
				}

				RegisterNotifyPropertyChangedEvent();

			}
		}

		#region Constructors

		/// <summary>
		/// Constructs a Comparison Tracker with <paramref name="currentValue"/> and optionally <paramref name="cloneFunc"/>.
		/// </summary>
		/// <param name="currentValue">The current value to monitor.</param>
		/// <param name="cloneFunc">The function to clone the <typeparamref name="TObject"/>.</param>
		public ComparisonTracker(TObject currentValue, Func<TObject, TObject> cloneFunc = null) : this(currentValue,
			AnalyzerSettings.DefaultSettings.Invoke(), cloneFunc)
		{ }

		/// <summary>
		/// Constructs a Comparison Tracker with <paramref name="currentValue"/>, <paramref name="analyzerSettings"/> and optionally <paramref name="cloneFunc"/>.
		/// </summary>
		/// <param name="currentValue">The current value to monitor.</param>
		/// <param name="analyzerSettings">Settings for the <see cref="AnalyzerBuilder{TInstance}.Build"/>.</param>
		/// <param name="cloneFunc">The function to clone the <typeparamref name="TObject"/>.</param>
		public ComparisonTracker(TObject currentValue, AnalyzerSettings analyzerSettings, Func<TObject, TObject> cloneFunc = null)
			: this(currentValue,
			AnalyzerBuilder<TObject>.Build(analyzerSettings).ToComposite(),
			cloneFunc)
		{ }

		/// <summary>
		/// Constructs a Comparison Tracker with <paramref name="currentValue"/> and optionally <paramref name="cloneFunc"/>.
		/// </summary>
		/// <param name="currentValue">The current value to monitor.</param>
		/// <param name="analyzer">The Analyzer.</param>
		/// <param name="cloneFunc">The function to clone the <typeparamref name="TObject"/>.</param>
		public ComparisonTracker(TObject currentValue, IObjectAnalyzer<TObject> analyzer, Func<TObject, TObject> cloneFunc = null)
		{
			_analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
			_history = new List<IComparisonHistory<TObject>>();
			_cloneFunc = cloneFunc;
			Current = currentValue;
		}

		#endregion

		/// <inheritdoc />
		public IEnumerable<DiffSnapshot> GetCurrentAnalysis()
		{
			ThrowIfDisposed();
			if (_currentAnalysis == null)
			{
				Analyze();
			}

			return _currentAnalysis;
		}

		/// <inheritdoc />
		public IEnumerable<IComparisonHistory<TObject>> GetHistory()
		{
			ThrowIfDisposed();
			return _history.AsEnumerable();
		}

		/// <inheritdoc />
		public void Analyze()
		{
			ThrowIfDisposed();
			_currentAnalysis = _analyzer.Analyze(Original, Current);
		}

		/// <inheritdoc />
		public bool HasChanges(bool autoAnalyze = false)
		{
			ThrowIfDisposed();
			if (autoAnalyze || _currentAnalysis == null)
			{
				Analyze();
			}

			return !IsPatched() && _currentAnalysis.HasChanges();
		}

		/// <inheritdoc />
		public bool IsPatched()
		{
			ThrowIfDisposed();
			return _currentAnalysis != null && _currentAnalysis.IsPatched;
		}

		/// <inheritdoc />
		public void Patch()
		{
			ThrowIfDisposed();
			if (_currentAnalysis == null)
			{
				Analyze();
			}

			if (!_currentAnalysis.IsPatched)
			{
				_currentAnalysis.Patch();
			}

			_history.Add(new ComparisonHistory<TObject>(CloneCurrentValue(), _currentAnalysis));
		}

		/// <inheritdoc />
		public void Reset(bool toCurrent = false)
		{
			ThrowIfDisposed();
			if (toCurrent)
			{
				Original = CloneValue(Current);
			}
			else
			{
				Current = CloneValue(Original);
			}
			_currentAnalysis = null;
		}

		/// <inheritdoc />
		public void RevertTo(IComparisonHistory<TObject> history)
		{
			ThrowIfDisposed();
			Current = CloneValue(history.Get());
			Analyze();
		}

		#region Clone

		private void CloneToOriginal()
		{
			_history.Add(new ComparisonHistory<TObject>(CloneCurrentValue(), Enumerable.Empty<DiffSnapshot>()));
			Original = CloneCurrentValue();
		}

		private TObject CloneCurrentValue() => CloneValue(Current);

		private TObject CloneValue(TObject value)
		{
			if (_cloneFunc != null)
			{
				return _cloneFunc.Invoke(value);
			}

			if (value == null)
			{
				return default;
			}

			if (value is ICloneable cloneable)
			{
				return (TObject)cloneable.Clone();
			}

			throw new ObjectComparisonException(Resources.Errors.CannotClone);
		}

		#endregion

		#region I Notify Property Changed

		private void RegisterNotifyPropertyChangedEvent()
		{
			if (Current is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
			}
		}

		// Maybe change the name of the method to something better.
		private void UnRegisterNotifyPropertyChangedEvent()
		{
			if (Current is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
			}
		}

		private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Analyze();
		}


		#endregion

		private void ThrowIfDisposed()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(nameof(ComparisonTracker<TObject>));
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			UnRegisterNotifyPropertyChangedEvent();
			_isDisposed = true;
		}

		#region Casts

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

		#endregion
	}
}