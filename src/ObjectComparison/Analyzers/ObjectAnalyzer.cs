using System;
using System.Collections.Generic;
using System.Diagnostics;
using ObjectComparison.Analyzers.Infos;
using ObjectComparison.Results;

namespace ObjectComparison.Analyzers
{
	[DebuggerDisplay("Object Analyzer: {_objectInfo.GetName()}")]
	public class ObjectAnalyzer<TInstance, TObject> : IObjectAnalyzer<TInstance>
	{
		private readonly ObjectInfo<TInstance, TObject> _objectInfo;

		public ObjectAnalyzer(ObjectInfo<TInstance, TObject> objectInfo)
		{
			_objectInfo = objectInfo ?? throw new ArgumentNullException(nameof(objectInfo));
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
			var newValue = _objectInfo.Get(targetInstance);
			var infoBuilder = new DiffSnapshot.Builder()
				.SetName(_objectInfo.GetName())
				.SetOriginalValue(originalValue)
				.SetNewValue(newValue);

			if (!_objectInfo.IsEqual(originalValue, newValue))
			{
				infoBuilder.HasChanges();
				_objectInfo.Set(originalInstance, newValue);
			}

			yield return infoBuilder.Build();
		}
	}
}