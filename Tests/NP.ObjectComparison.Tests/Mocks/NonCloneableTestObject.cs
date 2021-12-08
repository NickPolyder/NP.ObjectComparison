using System;

namespace NP.ObjectComparison.Tests.Mocks
{
	public class NonCloneableTestObject: IEquatable<NonCloneableTestObject>
	{
		public string FirstProperty { get; set; }

		public int SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public bool Equals(NonCloneableTestObject other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return FirstProperty == other.FirstProperty && SecondProperty == other.SecondProperty && ThirdProperty.Equals(other.ThirdProperty);
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

			if (obj.GetType() != typeof(NonCloneableTestObject))
			{
				return false;
			}

			return Equals((NonCloneableTestObject) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstProperty, SecondProperty, ThirdProperty);
		}
	}
}