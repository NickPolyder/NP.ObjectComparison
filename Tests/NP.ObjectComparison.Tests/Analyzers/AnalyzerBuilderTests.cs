using System.Linq;
using System.Reflection;
using AutoFixture;
using NP.ObjectComparison.Analyzers;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests.Analyzers;

public class AnalyzerBuilderTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	private IFixture _fixture;
	public AnalyzerBuilderTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		_fixture = new Fixture();
		_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
	}

	[Fact]
	public void BuildOnce_ShouldCreateCache()
	{
		// Arrange
		var analyzerType = typeof(AnalyzerBuilder<TestObjectWithNullable>);

		// Act
		var sut = AnalyzerBuilder<TestObjectWithNullable>.Build();

		// Assert
		sut.ShouldNotBeEmpty();
		var cachedPropertiesField = analyzerType.GetField("_cachedProperties", BindingFlags.Static | BindingFlags.NonPublic);
		cachedPropertiesField.ShouldNotBeNull();
		cachedPropertiesField.GetValue(null).ShouldNotBeNull();
	}


	[Fact]
	public void BuildTwice_ShouldUseTheCachedData()
	{
		// Arrange
		var analyzerType = typeof(AnalyzerBuilder<TestObjectWithNullable>);
		var initialisedData =  AnalyzerBuilder<TestObjectWithNullable>.Build().ToArray();

		// Act
		var sut = AnalyzerBuilder<TestObjectWithNullable>.Build().ToArray();

		// Assert
		sut.ShouldNotBeEmpty();
		var cachedPropertiesField = analyzerType.GetField("_cachedProperties", BindingFlags.Static | BindingFlags.NonPublic);
		cachedPropertiesField.ShouldNotBeNull();
		cachedPropertiesField.GetValue(null).ShouldNotBeNull();
	}
}