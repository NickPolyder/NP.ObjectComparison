using System;
using ObjectComparison.Analyzers.Infos;
using Shouldly;
using Xunit;

namespace ObjectComparison.Tests.Analyzers.Infos
{
	public class ObjectInfoTests
	{
		[Fact]
		public void Ctor_WhenGetNameFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ObjectInfo<object, object>(null, _ => null, (instance, value) => { }));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("getNameFunction");
		}

		[Fact]
		public void Ctor_WhenGetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ObjectInfo<object, object>(() => "", null, (instance, value) => { }));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("getterFunction");
		}

		[Fact]
		public void Ctor_WhenSetterFunctionIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ObjectInfo<object, object>(() => "", _ => null, null));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("setterAction");
		}

		[Fact]
		public void Ctor_WhenEqualsPredicateIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ObjectInfo<object, object>(() => "", _ => null,
				(instance, value) => { },
				null));
			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("equalsPredicate");
		}
	}
}