using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectPatcher.Tests.Mocks
{
	public class TestObjectWithArrays : ICloneable
	{
		public string[] FirstProperty { get; set; }

		public Dictionary<string, string> SecondProperty { get; set; }

		public DateTime ThirdProperty { get; set; }

		public List<OtherTestObject> FourthProperty { get; set; }

		public object Clone()
		{
			return new TestObjectWithArrays
			{
				FirstProperty = (string[])FirstProperty.Clone(),
				SecondProperty = SecondProperty.ToDictionary(key => key.Key, value => (string)value.Value.Clone()),
				ThirdProperty = ThirdProperty,
				FourthProperty = FourthProperty.Select(item=> (OtherTestObject)item.Clone()).ToList()
			};
		}
	}
}