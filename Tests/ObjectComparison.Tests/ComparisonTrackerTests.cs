using System;
using AutoFixture;
using ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectComparison.Tests
{
	public class ComparisonTrackerTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public ComparisonTrackerTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
			_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}

		[Fact]
		public void Ctor_WhenAnalyzerIsNull_ShouldThrowException()
		{
			// Arrange

			// Act
			var result = Record.Exception(() => new ComparisonTracker<OtherTestObject>(null, null, null));

			// Assert
			result.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("analyzer");
		}

		[Fact]
		public void ShouldDetectChanges()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			// Act
			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			// Assert
			sut.HasChanges().ShouldBeTrue();
		}
	}
}