using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{

	/// <summary>
	/// Analyzes Complex Dictionaries.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[DebuggerDisplay("Dictionary Analyzer: {ObjectInfo.GetName()}")]
	public class ComplexDictionaryAnalyzer<TInstance, TKey, TValue> : BaseDictionaryAnalyzer<TInstance, TKey, TValue>
	{
		private readonly IEnumerable<IObjectAnalyzer<TValue>> _analyzers;

		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <param name="analyzers"></param>
		/// <exception cref="System.ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		public ComplexDictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo, IEnumerable<IObjectAnalyzer<TValue>> analyzers) : base(objectInfo)
		{
			_analyzers = analyzers;
		}

		/// <inheritdoc />
		protected override IEnumerable<DiffSnapshot> HandleModifiedItems(IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> target)
		{
			var sameKeys = original.Keys
				.Where(originalKey => target.Keys.Contains(originalKey))
				.ToArray();

			if (sameKeys.Length == 0)
			{
				yield break;
			}

			foreach (var analyzer in _analyzers)
			{
				foreach (var key in sameKeys)
				{
					var originalItem = original[key];
					var targetItem = target[key];
					
					foreach (var objectItem in analyzer.Analyze(originalItem, targetItem))
					{
						yield return new DiffSnapshot.Builder(objectItem)
							.SetPrefix($"{ObjectInfo.GetName()}[{key}]")
							.Build();
					}
				}
			}
		}
	}
}