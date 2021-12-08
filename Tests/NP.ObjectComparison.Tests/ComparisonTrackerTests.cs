using System;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NP.ObjectComparison.Exceptions;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests
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
		public void Ctor_WhenCurrentValueIsNull_ShouldMakeOriginalDefault()
		{
			// Arrange

			// Act
			var sut = new ComparisonTracker<OtherTestObject>(null);

			// Assert
			sut.Original.ShouldBeNull();
		}

		[Fact]
		public void Ctor_WhenObjectIsNotCloneable_ShouldThrowException()
		{
			// Arrange
			var current = _fixture.Create<NonCloneableTestObject>();
			// Act
			var result = Record.Exception(() => new ComparisonTracker<NonCloneableTestObject>(current));

			// Assert
			result.ShouldBeOfType<ObjectComparisonException>().Message.ShouldBe(Resources.Errors.CannotClone);
		}

		[Fact]
		public void Ctor_WhenObjectIsNotCloneableAndHaveCloneFunction_ShouldClone()
		{
			// Arrange
			var current = _fixture.Create<NonCloneableTestObject>();
			// Act
			var sut = new ComparisonTracker<NonCloneableTestObject>(current, cur => new NonCloneableTestObject
			{
				FirstProperty = cur.FirstProperty,
				SecondProperty = cur.SecondProperty,
				ThirdProperty = cur.ThirdProperty
			});

			// Assert
			sut.Original.ShouldBe(sut.Current);
		}

		[Fact]
		public void Current_WhenOriginalIsSet_ShouldNotClone()
		{
			// Arrange
			var original = _fixture.Create<OtherTestObject>();
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(original);

			// Act
			sut.Current = current;

			// Assert
			sut.Original.ShouldBe(original);
			sut.Current.ShouldBe(current);
		}

		[Fact]
		public void Current_WhenOriginalIsNotSet_ShouldClone()
		{
			// Arrange
			var original = _fixture.Create<OtherTestObject>();
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(original);

			// Act
			var isOriginalSet = typeof(ComparisonTracker<OtherTestObject>).GetRuntimeFields()
				.First(item => item.Name == "_isOriginalSet");
			isOriginalSet.SetValue(sut, false);

			sut.Current = current;

			// Assert
			sut.Original.ShouldBe(current);
			sut.Current.ShouldBe(current);
		}

		[Fact]
		public void HasChanges_ShouldDetectChanges()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void HasChanges_WhenThereAreNoChanges_ShouldNotDetectChanges()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void IsPatched_WhenItsNotAnalyzed_ShouldReturnFalse()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			var result = sut.IsPatched();

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void IsPatched_WhenItsAnalyzedAndNotPatched_ShouldReturnFalse()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			sut.Analyze();
			var result = sut.IsPatched();

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void IsPatched_WhenItsAnalyzedAndPatched_ShouldReturnTrue()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			sut.Analyze();
			sut.Patch();

			var result = sut.IsPatched();

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void Patch_WhenNotAnalyzed_ShouldAnalyzeAndPatch()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			sut.Patch();

			// Assert
			sut.Original.ShouldBe(sut.Current);
			sut.IsPatched().ShouldBeTrue();
		}
		
		[Fact]
		public void Reset_ShouldOverrideOriginalValue()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			sut.Reset();
			// Assert
			sut.Original.ShouldBe(sut.Current);
		}

		[Fact]
		public void CastToObject_WhenTheTrackerIsNull_ShouldReturnNull()
		{
			// Arrange
			ComparisonTracker<OtherTestObject> sut = null;

			// Act
			OtherTestObject result = sut;
			// Assert
			result.ShouldBeNull();
		}

		[Fact]
		public void CastToObject_WhenTheTrackerIsNotNull_ShouldReturnObject()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			// Act
			OtherTestObject result = sut;
			// Assert
			result.ShouldBe(current);
		}

		[Fact]
		public void CastToTracker_ShouldReturnTracker()
		{
			// Arrange
			var sut = _fixture.Create<OtherTestObject>();

			// Act
			ComparisonTracker<OtherTestObject> result = sut;
			// Assert
			result.Current.ShouldBe(sut);
		}
	}
}