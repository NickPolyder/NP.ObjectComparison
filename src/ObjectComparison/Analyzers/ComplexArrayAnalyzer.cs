using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	[DebuggerDisplay("Complex Array Analyzer: {ObjectInfo.GetName()}")]
	public class ComplexArrayAnalyzer<TInstance, TArray, TArrayOf> : BaseArrayAnalyzer<TInstance, TArray, TArrayOf>
	{
		private readonly IEnumerable<IObjectAnalyzer<TArrayOf>> _analyzers;
		public ComplexArrayAnalyzer(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo, IEnumerable<IObjectAnalyzer<TArrayOf>> analyzers) : base(objectInfo)
		{
			_analyzers = analyzers;
		}
		
		protected override IEnumerable<DiffSnapshot> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			foreach (var analyzer in _analyzers)
			{
				for (int index = 0; index < originalArray.Length && index < targetArray.Length; index++)
				{
					var originalItem = originalArray[index];
					var targetItem = targetArray[index];
					foreach (var objectItem in analyzer.Analyze(originalItem, targetItem))
					{
						yield return new DiffSnapshot.Builder(objectItem)
							.SetPrefix($"{ObjectInfo.GetName()}[{index}]")
							.Build();
					}
				}
			}
		}
	}
}