using System.Linq;
using AutoFixture;
using ObjectPatcher.Results;
using ObjectPatcher.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectPatcher.Tests
{
	public class DiffCompositeTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public DiffCompositeTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
		}


		[Fact]
		public void Diff_WhenThereAreChanges_ShouldReturnTrue()
		{
			// Arrange
			var sut = new DiffComposite<TestObject>(PropertyDiffBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Diff(originalValue, targetValue);

			// Assert
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Diff_WhenTheValuesAreTheSameReference_ShouldReturnFalse()
		{
			// Arrange
			var sut = new DiffComposite<TestObject>(PropertyDiffBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Diff(originalValue, originalValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Diff_WhenThereAreNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new DiffComposite<TestObject>(PropertyDiffBuilder<TestObject>.Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = (TestObject)originalValue.Clone();

			// Act
			var result = sut.Diff(originalValue, targetValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Diff_WhenThereAreCollectionsAndNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new DiffComposite<TestObjectWithArrays>(PropertyDiffBuilder<TestObjectWithArrays>.Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			// Act
			var result = sut.Diff(originalValue, targetValue).ToList();

			// Assert
			foreach (var diffItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {diffItem.Name}, Has Changes: {diffItem.HasChanges}");
			}
			result.HasChanges().ShouldBeFalse();
		}
	}
}