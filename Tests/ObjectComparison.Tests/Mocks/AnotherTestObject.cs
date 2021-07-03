using System;

namespace ObjectComparison.Tests.Mocks
{
	public class AnotherTestObject : ICloneable, IEquatable<AnotherTestObject>
	{
		public string AnotherFirstProperty { get; set; }

		public int AnotherSecondProperty { get; set; }

		public DateTime AnotherThirdProperty { get; set; }

		public object Clone()
		{
			return new AnotherTestObject
			{
				AnotherFirstProperty = AnotherFirstProperty,
				AnotherSecondProperty = AnotherSecondProperty,
				AnotherThirdProperty = AnotherThirdProperty
			};
		}

		public bool Equals(AnotherTestObject other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return AnotherFirstProperty == other.AnotherFirstProperty
			       && AnotherSecondProperty == other.AnotherSecondProperty
			       && AnotherThirdProperty.Equals(other.AnotherThirdProperty);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((AnotherTestObject)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(AnotherFirstProperty, AnotherSecondProperty, AnotherThirdProperty);
		}
	}
}