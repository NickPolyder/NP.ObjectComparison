using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	[DebuggerDisplay("Dictionary Analyzer: {ObjectInfo.GetName()}")]
	public class ComplexDictionaryAnalyzer<TInstance, TKey, TValue> : BaseDictionaryAnalyzer<TInstance, TKey, TValue>
	{
		private readonly IEnumerable<IObjectAnalyzer<TValue>> _analyzers;
		public ComplexDictionaryAnalyzer(DictionaryObjectInfo<TInstance, TKey, TValue> objectInfo, IEnumerable<IObjectAnalyzer<TValue>> analyzers) : base(objectInfo)
		{
			_analyzers = analyzers;
		}

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