using System;

namespace ObjectComparison.Results
{
	/// <summary>
	/// An object holding the Difference information about 2 objects.
	/// </summary>
	public class DiffSnapshot
	{
		/// <summary>
		/// The name of the object.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Does this Difference Snapshot have changes ?
		/// </summary>
		public bool HasChanges { get; }

		/// <summary>
		/// The Original Value.
		/// </summary>
		public object OriginalValue { get; }

		/// <summary>
		/// The New Value.
		/// </summary>
		public object NewValue { get; }
		
		private DiffSnapshot(string name, object originalValue, object newValue, bool hasChanges)
		{
			Name = name;
			OriginalValue = originalValue;
			NewValue = newValue;
			HasChanges = hasChanges;
		}

		/// <summary>
		/// Creates a <see cref="DiffSnapshot"/>.
		/// </summary>
		/// <param name="configure">An Action that builds the <see cref="Builder"/>.</param>
		/// <returns>The <see cref="DiffSnapshot"/> constructed by <paramref name="configure"/>.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="configure"/> is null.</exception>
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

		/// <summary>
		/// <see cref="DiffSnapshot"/> Builder.
		/// </summary>
		public class Builder
		{
			private string _prefix;
			private string _name;
			private bool _hasChanges = false;
			private object _originalValue;
			private object _newValue;

			/// <summary>
			/// Constructs this object.
			/// </summary>
			public Builder()
			{  }

			/// <summary>
			/// Constructs this object using an initial snapshot.
			/// </summary>
			/// <param name="initial"></param>
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

			/// <summary>
			/// Sets the prefix for this <see cref="DiffSnapshot"/>.
			/// </summary>
			/// <param name="prefix"></param>
			/// <returns>Self</returns>
			public Builder SetPrefix(string prefix)
			{
				_prefix = prefix;
				return this;
			}

			/// <summary>
			/// Sets the name for this <see cref="DiffSnapshot"/>.
			/// </summary>
			/// <param name="name"></param>
			/// <returns>Self</returns>
			public Builder SetName(string name)
			{
				_name = name;
				return this;
			}

			/// <summary>
			/// Sets the Original Value for this <see cref="DiffSnapshot"/>.
			/// </summary>
			/// <param name="originalValue"></param>
			/// <returns>Self</returns>
			public Builder SetOriginalValue(object originalValue)
			{
				_originalValue = originalValue;
				return this;
			}

			/// <summary>
			/// Sets the HasChanges Value for this <see cref="DiffSnapshot"/>.
			/// </summary>
			/// <param name="hasChanges"></param>
			/// <returns>Self</returns>
			public Builder HasChanges(bool hasChanges = true)
			{
				_hasChanges = hasChanges;
				return this;
			}

			/// <summary>
			/// Sets the New Value for this <see cref="DiffSnapshot"/>.
			/// </summary>
			/// <param name="newValue"></param>
			/// <returns>Self</returns>
			public Builder SetNewValue(object newValue)
			{
				_newValue = newValue;
				return this;
			}

			/// <summary>
			/// Constructs a <see cref="DiffSnapshot" /> based on this <see cref="Builder"/>.
			/// </summary>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException">When the name is null or whitespace.</exception>
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