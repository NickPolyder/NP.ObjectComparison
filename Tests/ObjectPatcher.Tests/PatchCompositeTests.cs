using System.Linq;
using AutoFixture;
using ObjectPatcher.Results;
using ObjectPatcher.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ObjectPatcher.Tests
{
	public class PatchCompositeTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private IFixture _fixture;
		public PatchCompositeTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();
		}


		[Fact]
		public void Patch_WhenThereAreChanges_ShouldReturnTrue()
		{
			// Arrange
			var sut = new PatchComposite<TestObject>(new PropertyPatchBuilder<TestObject>().Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Patch(originalValue, targetValue);

			// Assert
			result.HasChanges().ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenTheValuesAreTheSameReference_ShouldReturnFalse()
		{
			// Arrange
			var sut = new PatchComposite<TestObject>(new PropertyPatchBuilder<TestObject>().Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();

			// Act
			var result = sut.Patch(originalValue, originalValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Patch_WhenThereAreNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new PatchComposite<TestObject>(new PropertyPatchBuilder<TestObject>().Build().ToArray());

			var originalValue = _fixture.Create<TestObject>();
			var targetValue = (TestObject)originalValue.Clone();

			// Act
			var result = sut.Patch(originalValue, targetValue);

			// Assert
			result.HasChanges().ShouldBeFalse();
		}

		[Fact]
		public void Patch_WhenThereAreCollectionsAndNoChanges_ShouldReturnFalse()
		{
			// Arrange
			var sut = new PatchComposite<TestObjectWithArrays>(new PropertyPatchBuilder<TestObjectWithArrays>().Build().ToArray());

			var originalValue = _fixture.Create<TestObjectWithArrays>();
			var targetValue = (TestObjectWithArrays)originalValue.Clone();

			// Act
			var result = sut.Patch(originalValue, targetValue).ToList();

			// Assert
			foreach (var patchItem in result)
			{
				_testOutputHelper.WriteLine($"Item Name: {patchItem.Name}, Has Changes: {patchItem.HasChanges}");
			}
			result.HasChanges().ShouldBeFalse();
		}
	}
}