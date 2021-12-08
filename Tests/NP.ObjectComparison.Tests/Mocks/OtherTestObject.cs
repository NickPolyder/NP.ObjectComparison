using System;

namespace NP.ObjectComparison.Tests.Mocks
{
	public class OtherTestObject : ICloneable, IEquatable<OtherTestObject>
	{
		public string OtherFirstProperty { get; set; }

		public int OtherSecondProperty { get; set; }

		public DateTime OtherThirdProperty { get; set; }

		public AnotherTestObject OtherFourthProperty { get; set; }
		public object Clone()
		{
			return new OtherTestObject
			{
				OtherFirstProperty = OtherFirstProperty,
				OtherSecondProperty = OtherSecondProperty,
				OtherThirdProperty = OtherThirdProperty,
				OtherFourthProperty = (AnotherTestObject)OtherFourthProperty?.Clone()
			};
		}

		public bool Equals(OtherTestObject other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return OtherFirstProperty == other.OtherFirstProperty 
			       && OtherSecondProperty == other.OtherSecondProperty
			       && OtherThirdProperty.Equals(other.OtherThirdProperty)
			       && object.Equals(OtherFourthProperty, other.OtherFourthProperty);
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

			return Equals((OtherTestObject) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(OtherFirstProperty, OtherSecondProperty, OtherThirdProperty, OtherFourthProperty);
		}
	}
}