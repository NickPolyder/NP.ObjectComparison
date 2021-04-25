using System;
using Shouldly;
using Xunit;

namespace ObjectPatcher.Tests
{
	public class PropertyPatchTests
	{

		[Fact]
		public void Ctor_WhenGetNameFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(null, _ => null, (instance, value) => { }));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("getNameFunction");
		}

		[Fact]
		public void Ctor_WhenGetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(() => "", null, (instance, value) => { }));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("getterFunction");
		}

		[Fact]
		public void Ctor_WhenSetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(() => "", _ => null, null));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("setterAction");
		}

		[Fact]
		public void Ctor_WhenEqualsPredicateIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyPatch<object, object>(() => "", _ => null,
				(instance, value) => { },
				null));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("equalsPredicate");
		}
	}
}