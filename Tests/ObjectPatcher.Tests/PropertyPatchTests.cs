using System;
using System.Linq.Expressions;
using System.Reflection;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace ObjectPatcher.Tests
{
	public class PropertyPatchTests
	{
		[Fact]
		public void Ctor_WhenGetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(null, (instance, value) => { }));
			// Assert
			var ex = result.ShouldBeOfType<ArgumentNullException>();
			ex.ParamName.ShouldBe("getterFunction");
		}

		[Fact]
		public void Ctor_WhenSetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(_ => null, null));
			// Assert
			var ex = result.ShouldBeOfType<ArgumentNullException>();
			ex.ParamName.ShouldBe("setterAction");
		}

		[Fact]
		public void Ctor_WhenEqualsPredicateIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(_ => null,
				(instance, value) => { },
				null));
			// Assert
			var ex = result.ShouldBeOfType<ArgumentNullException>();
			ex.ParamName.ShouldBe("equalsPredicate");
		}
	}
}