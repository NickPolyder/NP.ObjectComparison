using System;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NP.ObjectComparison.Exceptions;
using NP.ObjectComparison.Results;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests
{
	public class ComparisonTrackerTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		private readonly IFixture _fixture;
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
			var result = Record.Exception(() => new ComparisonTracker<OtherTestObject>(null,
				(IObjectAnalyzer<OtherTestObject>)null,
				null));

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
		public void HasChanges_WhenAutoAnalyzeIsProvided_ShouldRunAnalysis()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);
			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			// Act
			var result = sut.HasChanges(true);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void HasChanges_WhenPatched_ShouldDisregardTheCurrentAnalysis()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);
			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			sut.Patch();

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
		public void Reset_WhenToCurrentIsTrue_ShouldOverrideCurrentValue()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			sut.Reset(true);

			// Assert
			sut.Current.ShouldBe(sut.Original);
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

		[Fact]
		public void GetCurrentAnalysis_WhenNotAnalyzed_ShouldRunTheAnalysisAndReturnTheValue()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			// Act
			var result = sut.GetCurrentAnalysis().ToArray();

			// Assert
			result.HasChanges().ShouldBe(true);
			result.First(item => item.HasChanges).Name.ShouldBe($"{nameof(OtherTestObject.OtherFirstProperty)}");
		}

		[Fact]
		public void GetCurrentAnalysis_WhenAnalyzed_ShouldReturnTheValue()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current)
			{
				Current = { OtherFirstProperty = _fixture.Create<string>() }
			};

			sut.Analyze();

			// Act
			var result = sut.GetCurrentAnalysis().ToArray();

			// Assert
			result.HasChanges().ShouldBe(true);
			result.First(item => item.HasChanges).Name.ShouldBe($"{nameof(OtherTestObject.OtherFirstProperty)}");
		}

		[Fact]
		public void GetHistory_WhenNoChangesHaveBeenMade_ShouldReturnTheInitialHistoryItem()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			// Act
			var result = sut.GetHistory().ToArray();

			// Assert
			result.ShouldHaveSingleItem();
			var history = result.First();
			history.Get().ShouldBe(current);
			history.GetDiff().ShouldBeEmpty();
		}

		[Fact]
		public void GetHistory_ShouldHaveAnItemForEveryPatchCalled()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			sut.Patch();

			sut.Current.OtherSecondProperty = _fixture.Create<int>();

			sut.Patch();

			// Act
			var result = sut.GetHistory().ToArray();

			// Assert
			// 2 Patch called + 1 for the initial.

			result.Length.ShouldBe(2 + 1);
			var history = result.Last();
			history.Get().ShouldBe(current);
			history.GetDiff().ShouldNotBeEmpty();
		}

		[Fact]
		public void RevertTo_ShouldReplaceCurrentWithACopyOfThatHistoryItem()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			sut.Patch();

			sut.Current.OtherSecondProperty = _fixture.Create<int>();

			sut.Patch();

			sut.Current.OtherFourthProperty = _fixture.Create<AnotherTestObject>();

			sut.Patch();

			var historyItems = sut.GetHistory().ToArray();

			// Act
			var randomIndex = new Random().Next(0, historyItems.Length - 2);
			var selectedHistory = historyItems[randomIndex];

			sut.RevertTo(selectedHistory);

			// Assert
			sut.Current.ShouldBe(selectedHistory.Get());
		}

		[Fact]
		public void RevertTo_ShouldCallAnalyze()
		{
			// Arrange
			var current = _fixture.Create<OtherTestObject>();
			var sut = new ComparisonTracker<OtherTestObject>(current);

			sut.Current.OtherFirstProperty = _fixture.Create<string>();

			sut.Patch();

			sut.Current.OtherSecondProperty = _fixture.Create<int>();

			sut.Patch();

			sut.Current.OtherFourthProperty = _fixture.Create<AnotherTestObject>();

			sut.Patch();

			var historyItems = sut.GetHistory().ToArray();

			// Act
			var randomIndex = new Random().Next(0, historyItems.Length - 2);
			var selectedHistory = historyItems[randomIndex];

			var previousAnalysis = sut.GetCurrentAnalysis();

			sut.RevertTo(selectedHistory);

			var result = sut.GetCurrentAnalysis();

			// Assert
			result.ShouldNotBe(previousAnalysis);
		}

		[Fact]
		public void WhenObjectCanNotifyOnChanges_ShouldSubscribeToTheEvent()
		{
			// Arrange
			var current = _fixture.Create<NotifyChangesTestObject>();
			var sut = new ComparisonTracker<NotifyChangesTestObject>(current);

			// Act
			current.Age = _fixture.Create<int>();

			// Assert
			var analysis = sut.GetCurrentAnalysis().ToArray();

			var changedValue = analysis.FirstOrDefault(item => item.Name == nameof(current.Age));

			changedValue.ShouldNotBeNull();

			changedValue.HasChanges.ShouldBeTrue();
			changedValue.OriginalValue.ShouldBe(sut.Original.Age);
			changedValue.NewValue.ShouldBe(current.Age);
		}

		[Fact]
		public void WhenObjectCanNotifyOnChanges_AndSetANewCurrent_ShouldRemoveTheOldEventAndSubscribeToTheNewOne()
		{
			// Arrange
			var current = _fixture.Create<NotifyChangesTestObject>();
			var sut = new ComparisonTracker<NotifyChangesTestObject>(current);

			// Act
			sut.Current = _fixture.Create<NotifyChangesTestObject>();

			// Assert

			var previousObjInvocationList = current.GetInvocationList();
			previousObjInvocationList.ShouldBeNull();

			var invocationList = sut.Current.GetInvocationList();
			invocationList.ShouldNotBeEmpty();
			invocationList.Length.ShouldBe(1);

			var analysis = sut.GetCurrentAnalysis().ToArray();

			analysis.ShouldAllBe(item => item.HasChanges);
		}


		[Fact]
		public void WhenDisposingTheTracker_ShouldRemoveEvents()
		{
			// Arrange
			var current = _fixture.Create<NotifyChangesTestObject>();
			var sut = new ComparisonTracker<NotifyChangesTestObject>(current);

			// Act
			sut.Dispose();

			// Assert
			var invocationList = current.GetInvocationList();
			invocationList.ShouldBeNull();
		}

		[Fact]
		public void WhenDisposingTheSecondTime_ItShouldShortCircuit()
		{
			// Arrange
			var current = _fixture.Create<NotifyChangesTestObject>();
			var sut = new ComparisonTracker<NotifyChangesTestObject>(current);
			sut.Dispose();

			// Act
			sut.Dispose();

			// Assert
			var invocationList = current.GetInvocationList();
			invocationList.ShouldBeNull();
		}

		[Fact]
		public void WhenDisposed_ShouldThrowObjectDisposedException()
		{
			// Arrange
			var current = _fixture.Create<NotifyChangesTestObject>();
			var sut = new ComparisonTracker<NotifyChangesTestObject>(current);
			sut.Dispose();

			// Act
			foreach (var action in new Action[]
					 {
						 () => sut.HasChanges(),
						 () => sut.Patch(),
						 () => sut.Analyze(),
						 () => sut.Reset(),
						 () => sut.IsPatched(),
						 () => sut.GetHistory(),
						 () => sut.GetCurrentAnalysis(),
						 () =>
						 {
							 var o = sut.Original;
						 },
						 () =>
						 {
							 var c = sut.Current;
						 },
						 () => sut.RevertTo(sut.GetHistory().FirstOrDefault())

					 })
			{
				// Act
				var exception = Record.Exception(action);

				// Assert
				exception.ShouldNotBeNull();
				exception.ShouldBeOfType<ObjectDisposedException>().ObjectName.ShouldBe(nameof(ComparisonTracker<NotifyChangesTestObject>));
			}

		}
	}
}