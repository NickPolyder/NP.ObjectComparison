using AutoFixture;
using NP.ObjectComparison.Analyzers.Infos;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests.Analyzers.Infos
{
	public class ObjectInfoBuilderTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public ObjectInfoBuilderTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
			_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}
		[Fact]
		public void WhenObjectInfoIsOnNullableType_ShouldBeAbleToWrite()
		{
			// Arrange
			var propertyInfo = typeof(TestObjectWithNullable).GetProperty(nameof(TestObjectWithNullable.SecondProperty));
			var sut = (ObjectInfo<TestObjectWithNullable, int?>)ObjectInfoBuilder<TestObjectWithNullable>.Build(propertyInfo);

			var originalValue = _fixture.Create<TestObjectWithNullable>();

			// Act
			var result = Record.Exception(() => sut.Set(originalValue, 1));

			// Assert
			result.ShouldBeNull();
		}
	}
}