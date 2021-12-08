using System;
using NP.ObjectComparison.Attributes;

namespace NP.ObjectComparison.Tests.Mocks
{
	public class SkipTestObject : ICloneable, IEquatable<SkipTestObject>
	{
		[SkipAnalyze]
		public string FirstProperty { get; set; }

		public int SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public SkipOtherTestObject FourthProperty { get; set; }

		public TestObject FifthProperty { get; set; }
		public object Clone()
		{
			return new SkipTestObject
			{
				FirstProperty = FirstProperty,
				SecondProperty = SecondProperty,
				ThirdProperty = ThirdProperty,
				FourthProperty = (SkipOtherTestObject)FourthProperty?.Clone(),
				FifthProperty = (TestObject)FifthProperty?.Clone()
			};
		}

		public bool Equals(SkipTestObject other)
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

			return Equals((SkipTestObject)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstProperty, SecondProperty, ThirdProperty, FourthProperty, FifthProperty);
		}
	}
}