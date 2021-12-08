using System;
using System.Diagnostics;
using NP.ObjectComparison.Analyzers.Infos;
using NP.ObjectComparison.Results;

namespace NP.ObjectComparison.Analyzers
{
	/// <summary>
	/// The object analyzer of <typeparamref name="TInstance"/> of type: <typeparamref name="TObject"/>.
	/// </summary>
	/// <typeparam name="TInstance">The instance to be analyzed.</typeparam>
	/// <typeparam name="TObject">The <see cref="System.Reflection.PropertyInfo.PropertyType"/> to be analyzed.</typeparam>
	[DebuggerDisplay("Object Analyzer: {_objectInfo.GetName()}")]
	public class ObjectAnalyzer<TInstance, TObject> : IObjectAnalyzer<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		/// <summary>
		/// Constructs this object.
		/// </summary>
		/// <param name="objectInfo"></param>
		/// <exception cref="ArgumentNullException">When the <paramref name="objectInfo"/> is null.</exception>
		public ObjectAnalyzer(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
		}

		/// <inheritdoc />
		public IDiffAnalysisResult Analyze(TInstance originalInstance, TInstance targetInstance)
		{
			var diffAnalysis = new DiffAnalysisResult();
			if (originalInstance == null || targetInstance == null)
			{
				return diffAnalysis;
			}
			
			var originalValue = _objectInfo.Get(originalInstance);
			var newValue = _objectInfo.Get(targetInstance);
			var infoBuilder = new DiffSnapshot.Builder()
				.SetName(_objectInfo.GetName())
				.SetOriginalValue(originalValue)
				.SetNewValue(newValue);

			if (!_objectInfo.IsEqual(originalValue, newValue))
			{
				infoBuilder.HasChanges();
				diffAnalysis.AddPatchAction(() => _objectInfo.Set(originalInstance, newValue));
			}

			diffAnalysis.Add(infoBuilder.Build());
			
			return diffAnalysis;
		}
	}
}