using System;

namespace ObjectComparison.Tests.Mocks
{
	public class TestObject : ICloneable
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
	}
}