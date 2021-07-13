using System.Linq;
using AutoFixture;
using ObjectComparison.Analyzers;
using ObjectComparison.Results;
using ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectComparison.Tests.Analyzers
{
	public class AnalyzerCompositeTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public AnalyzerCompositeTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
			_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}
		
		[Fact]
		public void Analyze_WhenThereAreChanges_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObject>(AnalyzerBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			originalValue.FifthProperty = _fixture.Create<TestObject>();
			originalValue.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			originalValue.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			originalValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			originalValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			originalValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			var targetValue = _fixture.Create<TestObject>();
			targetValue.FifthProperty = _fixture.Create<TestObject>();
			targetValue.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			targetValue.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			targetValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			targetValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();
			targetValue.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty.FifthProperty = _fixture.Create<TestObject>();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}

			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Analyze_WhenThereAreChangesAndCallPatch_ShouldPatchObjects()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObject>(AnalyzerBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Analyze(originalValue, targetValue);
			result.Patch();

			// Assert
			result.IsPatched.ShouldBeTrue();
			result.HasChanges().ShouldBeTrue();
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			originalValue.ShouldBe(targetValue);
		}

		[Fact]
		public void Analyze_WhenTheValuesAreTheSameReference_ShouldReturnFalse()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObject>(AnalyzerBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Analyze(originalValue, originalValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Analyze_WhenThereAreNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObject>(AnalyzerBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = (TestObject)originalValue.Clone();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Analyze_WhenThereAreCollectionsAndNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Analyze_WhenThereAreCollectionsChanges_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			targetValue.FirstProperty[0] = _fixture.Create<string>();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Analyze_WhenThereAreCollectionsHasAddedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			var addItemToArray = targetValue.FirstProperty.ToList();

			addItemToArray.Add(_fixture.Create<string>());

			targetValue.FirstProperty = addItemToArray.ToArray();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Analyze_WhenThereAreCollectionsHasDeletedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			var deleteItemArray = targetValue.FirstProperty.ToList();

			deleteItemArray.RemoveAt(0);

			targetValue.FirstProperty = deleteItemArray.ToArray();

			// Act
			var result = sut.Analyze(originalValue, targetValue);

			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Analyze_WhenThereAreDictionariesWithDeletedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			 targetValue.SecondProperty.Remove(targetValue.SecondProperty.Keys.ToArray()[0]);
			
			// Act
			var result = sut.Analyze(originalValue, targetValue);
			
			// Assert
			foreach (var analyzeItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {analyzeItem.Name}, Has Changes: {analyzeItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}
	}
}