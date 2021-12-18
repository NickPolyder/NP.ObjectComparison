using System;

namespace NP.ObjectComparison.Tests.Mocks
{
	public class TestObjectWithNullable : ICloneable, IEquatable<TestObjectWithNullable>
	{
		public string FirstProperty { get; set; }

		public int? SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public object Clone()
		{
			return new TestObjectWithNullable
			{
				FirstProperty = FirstProperty,
				SecondProperty = SecondProperty,
				ThirdProperty = ThirdProperty
			};
		}

		public bool Equals(TestObjectWithNullable other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return FirstProperty == other.FirstProperty
				   && SecondProperty == other.SecondProperty
				   && ThirdProperty.Equals(other.ThirdProperty);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((TestObjectWithNullable)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstProperty, SecondProperty, ThirdProperty);
		}
	}
}