using System;
using NP.ObjectComparison.Analyzers;
using NP.ObjectComparison.Analyzers.Infos;
using Shouldly;
using Xunit;

namespace NP.ObjectComparison.Tests.Analyzers
{
	public class ObjectAnalyzersTests
	{
		[Fact]
		public void Ctor_WhenObjectInfoIsNull_ThrowArgumentNullException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ObjectAnalyzer<object, object>(null));

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
			var result = Record.Exception(() => new ObjectAnalyzer<object, object>(objectInfo));

			// Assert
			result.ShouldBeNull();
		}
	}
}