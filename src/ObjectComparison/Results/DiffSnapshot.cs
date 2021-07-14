using System;

namespace ObjectComparison.Results
{
	public class DiffSnapshot
	{
		public string Name { get; }

		public bool HasChanges { get; }

		public object OriginalValue { get; }

		public object NewValue { get; }
		
		private DiffSnapshot(string name, object originalValue, object newValue, bool hasChanges)
		{
			Name = name;
			OriginalValue = originalValue;
			NewValue = newValue;
			HasChanges = hasChanges;
		}

		public static DiffSnapshot Create(Action<Builder> configure)
		{
			if (configure == null)
			{
				throw new ArgumentNullException(nameof(configure));
			}

			var builder = new Builder();
			configure(builder);
			return builder.Build();
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

			public Builder(DiffSnapshot initial)
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

			public Builder HasChanges(bool hasChanges = true)
			{
				_hasChanges = hasChanges;
				return this;
			}

			public Builder SetNewValue(object newValue)
			{
				_newValue = newValue;
				return this;
			}

			public DiffSnapshot Build()
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
				return new DiffSnapshot(name, _originalValue, _newValue, _hasChanges);
			}
		}
	}
}