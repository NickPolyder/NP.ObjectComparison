using System;

namespace ObjectPatcher.Results
{
	public class PatchItem
	{
		public string Name { get; }

		public bool HasChanges { get; }

		public object OriginalValue { get; }

		public object NewValue { get; }

		private PatchItem(string name, object originalValue, object newValue, bool hasChanges)
		{
			Name = name;
			OriginalValue = originalValue;
			NewValue = newValue;
			HasChanges = hasChanges;
		}
		public class Builder
		{
			private string _name;
			private bool _hasChanges = false;
			private object _originalValue;
			private object _newValue;

			public Builder SetName(string name)
			{
				_name = name;
				return this;
			}

			public Builder SetOriginalValue(object originalValue)
			{
				_originalValue = originalValue;
				return this;
			}

			public Builder HasChanges()
			{
				_hasChanges = true;
				return this;
			}

			public Builder SetNewValue(object newValue)
			{
				_newValue = newValue;
				return this;
			}

			public PatchItem Build()
			{
				if (string.IsNullOrWhiteSpace(_name))
				{
					throw new ArgumentNullException(nameof(_name));
				}

				return new PatchItem(_name, _originalValue, _newValue, _hasChanges);
			}
		}
	}


}