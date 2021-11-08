using AutoFixture;
using ObjectComparison.Analyzers.Settings;
using ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectComparison.Tests.Analyzers.Settings
{
	public class SkipAnalyzeSettingsTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public SkipAnalyzeSettingsTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
		}

		[Fact]
		public void Skip_MemberExpression_ShouldSkipTheProperty()
		{
			// Arrange
			var sut = new SkipAnalyzeSettings();

			// Act
			var result = sut.Skip<TestObject, string>(model => model.FirstProperty);

			// Assert
			result.IsSkipped(typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty))).ShouldBeTrue();
		}
	}
}