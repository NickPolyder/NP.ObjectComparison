using System;

namespace ObjectComparison.Tests.Mocks
{
	public class TestObject : ICloneable, IEquatable<TestObject>
	{
		public string FirstProperty { get; set; }

		public int SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public OtherTestObject FourthProperty { get; set; }

		public TestObject FifthProperty { get; set; }
		public object Clone()
		{
			return new TestObject
			{
				FirstProperty = FirstProperty,
				SecondProperty = SecondProperty,
				ThirdProperty = ThirdProperty,
				FourthProperty = (OtherTestObject)FourthProperty?.Clone(),
				FifthProperty = (TestObject)FifthProperty?.Clone()
			};
		}

		public bool Equals(TestObject other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return FirstProperty == other.FirstProperty && SecondProperty == other.SecondProperty && ThirdProperty.Equals(other.ThirdProperty) && Equals(FourthProperty, other.FourthProperty) && Equals(FifthProperty, other.FifthProperty);
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

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((TestObject) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstProperty, SecondProperty, ThirdProperty, FourthProperty, FifthProperty);
		}
	}
}