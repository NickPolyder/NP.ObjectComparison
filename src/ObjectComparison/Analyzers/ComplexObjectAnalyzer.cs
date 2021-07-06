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

		public IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			var diffAnalysisResult = new DiffAnalysisResult();
			
			if (originalInstance == null || targetInstance == null)
			{
				return diffAnalysisResult;
			}

			var originalValue = _objectInfo.Get(originalInstance);
			var targetValue = _objectInfo.Get(targetInstance);
			
			foreach (var analyzer in _analyzers)
			{
				var analyzerResult = analyzer.Analyze(originalValue, targetValue);
				foreach (var objectItem in analyzerResult)
				{
					diffAnalysisResult.Add(new DiffSnapshot.Builder(objectItem)
						.SetPrefix(_objectInfo.GetName())
						.Build());
				}

				diffAnalysisResult.MergePatchAction(new Action(analyzerResult.Patch));
			}

			return diffAnalysisResult;
		}
	}
}