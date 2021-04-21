using System;

namespace ObjectPatcher.Tests
{
	public class TestObject : ICloneable
	{
		public string FirstProperty { get; set; }

		public int SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public OtherTestObject FourthProperty { get; set; }

		public object Clone()
		{
			return new TestObject
			{
				FirstProperty = FirstProperty,
				SecondProperty = SecondProperty,
				ThirdProperty = ThirdProperty,
				FourthProperty = (OtherTestObject)FourthProperty.Clone()
			};
		}
	}
	
	public class OtherTestObject : ICloneable, IEquatable<OtherTestObject>
	{
		public string FirstProperty { get; set; }

		public int SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public object Clone()
		{
			return new OtherTestObject
			{
				FirstProperty = FirstProperty,
				SecondProperty = SecondProperty,
				ThirdProperty = ThirdProperty
			};
		}

		public bool Equals(OtherTestObject other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return FirstProperty == other.FirstProperty && SecondProperty == other.SecondProperty && ThirdProperty.Equals(other.ThirdProperty);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((OtherTestObject) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstProperty, SecondProperty, ThirdProperty);
		}
	}
}