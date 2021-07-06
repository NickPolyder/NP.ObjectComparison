using System.Linq;
using AutoFixture;
using ObjectComparison.Analyzers;
using ObjectComparison.Results;
using ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectComparison.Tests.Patch
{
	public class PatchCompositeTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public PatchCompositeTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
			_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}
		
		[Fact]
		public void Patch_WhenThereAreChanges_ShouldReturnTrue()
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
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenTheValuesAreTheSameReference_ShouldReturnFalse()
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
		public void Patch_WhenThereAreNoChanges_ShouldReturnFalse()
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
		public void Patch_WhenThereAreCollectionsAndNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			// Act
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Patch_WhenThereAreCollectionsChanges_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			targetValue.FirstProperty[0] = _fixture.Create<string>();

			// Act
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenThereAreCollectionsHasAddedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			var addItemToArray = targetValue.FirstProperty.ToList();

			addItemToArray.Add(_fixture.Create<string>());

			targetValue.FirstProperty = addItemToArray.ToArray();

			// Act
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenThereAreCollectionsHasDeletedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			var deleteItemArray = targetValue.FirstProperty.ToList();

			deleteItemArray.RemoveAt(0);

			targetValue.FirstProperty = deleteItemArray.ToArray();

			// Act
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenThereAreDictionariesWithDeletedItems_ShouldReturnTrue()
		{
			// Arrange
			var sut = new AnalyzerComposite<TestObjectWithArrays>(AnalyzerBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			 targetValue.SecondProperty.Remove(targetValue.SecondProperty.Keys.ToArray()[0]);
			
			// Act
			var result = sut.Analyze(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeTrue();
		}
	}
}