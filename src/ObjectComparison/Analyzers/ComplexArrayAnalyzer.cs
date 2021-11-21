using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{

	/// <summary>
	/// Analyzes Complex Arrays.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TArray"></typeparam>
	/// <typeparam name="TArrayOf"></typeparam>
	[DebuggerDisplay("Complex Array Analyzer: {ObjectInfo.GetName()}")]
	public class ComplexArrayAnalyzer<TInstance, TArray, TArrayOf> : BaseArrayAnalyzer<TInstance, TArray, TArrayOf>
	{
		private readonly IEnumerable<IObjectAnalyzer<TArrayOf>> _analyzers;

		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <param name="analyzers"></param>
		/// <exception cref="System.ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		public ComplexArrayAnalyzer(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo, IEnumerable<IObjectAnalyzer<TArrayOf>> analyzers) : base(objectInfo)
		{
			_analyzers = analyzers;
		}

		/// <inheritdoc />
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