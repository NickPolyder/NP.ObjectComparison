using System;
using Shouldly;
using Xunit;

namespace ObjectPatcher.Tests
{
	public class PropertyDiffTests
	{
		[Fact]
		public void Ctor_WhenObjectInfoIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new PropertyDiff<object, object>(null));

			// Assert
			result.ShouldBeOfType<ArgumentNullException>()
				.ParamName.ShouldBe("objectInfo");
		}

		[Fact]
		public void Ctor_WhenObjectInfoIsNotNull_ThrowArgumentNullException()
		{
			// Arrange
			var objectInfo = new ObjectInfo<object, object>(() => "", _ => null, (x,y) => { });

			// Act
			var result = Record.Exception(() => new PropertyDiff<object, object>(objectInfo));

			// Assert
			result.ShouldBeNull();
		}
	}
}