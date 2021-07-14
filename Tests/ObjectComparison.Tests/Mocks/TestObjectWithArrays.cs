using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectComparison.Tests.Mocks
{
	public class TestObjectWithArrays : ICloneable
	{
		public string[] FirstProperty { get; set; }

		public Dictionary<string, string> SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public List<OtherTestObject> FourthProperty { get; set; }

		public Dictionary<string, OtherTestObject> FifthProperty { get; set; }

		public List<string> SixthProperty { get; set; }

		public object Clone()
		{
			return new TestObjectWithArrays
			{
				FirstProperty = (string[])FirstProperty.Clone(),
				SecondProperty = SecondProperty?.ToDictionary(key => key.Key, value => (string)value.Value.Clone()),
				ThirdProperty = ThirdProperty,
				FourthProperty = FourthProperty?.Select(item=> (OtherTestObject)item.Clone()).ToList(),
				FifthProperty = FifthProperty?.ToDictionary(key => key.Key, value => (OtherTestObject)value.Value.Clone()),
				SixthProperty = SixthProperty?.Select(item => (string) item.Clone()).ToList()
			};
		}
	}
}