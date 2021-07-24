﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Analyzers
{
	public static class ObjectAnalyzerExtensions
	{
		public static IObjectAnalyzer<TInstance> ToComposite<TInstance>(
			this IEnumerable<IObjectAnalyzer<TInstance>> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException(nameof(enumerable));
			}

			return new AnalyzerComposite<TInstance>(enumerable.ToArray());
		}

		/// <summary>
		/// Creates a Composite Analyzer for <typeparamref name="TObject"/>.
		/// </summary>
		/// <typeparam name="TObject">The type the analyzer is for.</typeparam>
		/// <param name="obj">the instance of the instance the initializer is for.</param>
		/// <returns>A Composite Analyzer for <typeparamref name="TObject"/>.</returns>
		public static IObjectAnalyzer<TObject> GetAnalyzer<TObject>(this TObject obj)
		{
			return AnalyzerBuilder<TObject>.Build().ToComposite();
		}

		/// <summary>
		/// Creates a Comparison Tracker for <typeparamref name="TObject"/>.
		/// </summary>
		/// <typeparam name="TObject">The type the comparison tracker is for.</typeparam>
		/// <param name="currentInstance">the current instance.</param>
		/// <param name="cloneFunction">(Optional) a function that will clone the <typeparamref name="TObject"/> instead of the default clone functionality.</param>
		/// <returns>A Comparison Tracker of <typeparamref name="TObject"/></returns>
		public static IComparisonTracker<TObject> GetComparisonTracker<TObject>(this TObject currentInstance, Func<TObject, TObject> cloneFunction = null)
		{
			return new ComparisonTracker<TObject>(currentInstance, currentInstance.GetAnalyzer(), cloneFunction);
		}
	}
}