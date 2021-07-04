﻿using System;

namespace ObjectComparison.Results
{
	public class ObjectItem
	{
		public string Name { get; }

		public bool HasChanges { get; }

		public object OriginalValue { get; }

		public object NewValue { get; }

		private ObjectItem(string name, object originalValue, object newValue, bool hasChanges)
		{
			Name = name;
			OriginalValue = originalValue;
			NewValue = newValue;
			HasChanges = hasChanges;
		}

		public class Builder
		{
			private string _prefix;
			private string _name;
			private bool _hasChanges = false;
			private object _originalValue;
			private object _newValue;

			public Builder()
			{  }

			public Builder(ObjectItem initial)
			{
				if (initial == null)
				{
					return;
				}

				_name = initial.Name;
				_hasChanges = initial.HasChanges;
				_originalValue = initial.OriginalValue;
				_newValue = initial.NewValue;
			}
			public Builder SetPrefix(string prefix)
			{
				_prefix = prefix;
				return this;
			}
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

			public ObjectItem Build()
			{
				if (string.IsNullOrWhiteSpace(_name))
				{
					throw new ArgumentNullException(nameof(_name));
				}

				var name = _name;
				if (!string.IsNullOrWhiteSpace(_prefix))
				{
					name = string.Join(".", new[]
					{
						_prefix,
						_name
					});
				}
				return new ObjectItem(name, _originalValue, _newValue, _hasChanges);
			}
		}
	}
}