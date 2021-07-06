using System;
using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	[DebuggerDisplay("Complex Object Analyzer: {_objectInfo.GetName()}")]
	public class ComplexObjectAnalyzer<TInstance, TObject> : IObjectAnalyzer<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;
		private readonly IEnumerable<IObjectAnalyzer<TObject>> _analyzers;

		public ComplexObjectAnalyzer(ObjectInfo<TInstance, TObject> objectInfo, IEnumerable<IObjectAnalyzer<TObject>> analyzers)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
			_analyzers = analyzers;
		}

		public IEnumerable<DiffSnapshot> Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			if (originalInstance == null)
			{
				yield break;
			}

			if (targetInstance == null)
			{
				yield break;
			}

			var originalValue = _objectInfo.Get(originalInstance);
			var targetValue = _objectInfo.Get(targetInstance);
			
			foreach (var analyzer in _analyzers)
			{
				foreach (var objectItem in analyzer.Analyze(originalValue, targetValue))
				{
					yield return new DiffSnapshot.Builder(objectItem)
						.SetPrefix(_objectInfo.GetName())
						.Build();
				}
			}
		}
	}
}