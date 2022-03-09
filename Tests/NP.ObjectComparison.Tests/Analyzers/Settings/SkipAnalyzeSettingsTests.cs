using AutoFixture;
using NP.ObjectComparison.Analyzers.Settings;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests.Analyzers.Settings
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
			var sut = new IgnoreSettings();

			// Act
			var result = sut.Ignore<TestObject, string>(model => model.FirstProperty);

			// Assert
			result.IsIgnored(typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty))).ShouldBeTrue();
		}
	}
}