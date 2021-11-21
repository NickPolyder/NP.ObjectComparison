using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	/// <summary>
	/// Analyzes an array.
	/// </summary>
	/// <typeparam name="TInstance"></typeparam>
	/// <typeparam name="TArray"></typeparam>
	/// <typeparam name="TArrayOf"></typeparam>
	[DebuggerDisplay("Array Analyzer: {ObjectInfo.GetName()}")]
	public class ArrayAnalyzer<TInstance, TArray, TArrayOf> : BaseArrayAnalyzer<TInstance, TArray, TArrayOf>
	{
		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <exception cref="System.ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		public ArrayAnalyzer(ArrayObjectInfo<TInstance, TArray, TArrayOf> objectInfo) : base(objectInfo)
		{
		}

		/// <inheritdoc />
		protected override IEnumerable<DiffSnapshot> HandleModifiedItems(TArrayOf[] originalArray, TArrayOf[] targetArray)
		{
			for (int index = 0; index < originalArray.Length && index < targetArray.Length; index++)
			{
				var originalItem = originalArray[index];
				var newItem = targetArray[index];
				var infoBuilder = new DiffSnapshot.Builder()
					.SetName($"{ObjectInfo.GetName()}[{index}]")
					.SetOriginalValue(originalItem)
					.SetNewValue(newItem);

				if (!ObjectInfo.IsItemEqual(originalItem, newItem))
				{
					infoBuilder.HasChanges();
				}

				yield return infoBuilder.Build();
			}
		}
	}
}