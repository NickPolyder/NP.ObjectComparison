using System.Collections.Concurrent;
using System;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NP.ObjectComparison.Analyzers.Settings;
using NP.ObjectComparison.Tests.Mocks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NP.ObjectComparison.Tests.Analyzers.Settings;

public class IgnoreSettingsTests
{
	private const string _ignoredTypeFieldName = "_ignoredTypes";
	private const string _ignoredPropertyFieldName = "_ignoredProperties";

	private readonly ITestOutputHelper _testOutputHelper;

	private IFixture _fixture;
	public IgnoreSettingsTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		_fixture = new Fixture();
	}

	[Fact]
	public void Ignore_MemberExpression_ShouldIgnoreTheProperty()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var propertyToIgnore = typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty));

		// Act
		var result = sut.Ignore<TestObject, string>(model => model.FirstProperty);

		// Assert
		result.IsIgnored(propertyToIgnore).ShouldBeTrue();
	}

	[Fact]
	public void Ignore_GenericType_ShouldIgnoreTheType()
	{
		// Arrange
		var sut = new IgnoreSettings();

		// Act
		var result = sut.Ignore<TestObject>();

		// Assert
		result.IsIgnored(typeof(TestObject)).ShouldBeTrue();
	}


	[Fact]
	public void Ignore_PropertyInfo_ShouldIgnoreTheProperty()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var propertyToIgnore = typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty));

		// Act
		var result = sut.Ignore(propertyToIgnore);

		// Assert
		result.IsIgnored(propertyToIgnore).ShouldBeTrue();
	}

	[Fact]
	public void Ignore_Type_ShouldIgnoreTheType()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var type = typeof(TestObject);
		// Act
		var result = sut.Ignore(type);

		// Assert
		result.IsIgnored(type).ShouldBeTrue();
	}

	[Fact(DisplayName = "When the ignored settings does not have the type in the ignored list and there is an [Ignore] attribute." +
						" It should add it on the spot.")]
	public void WhenTheTypeIsNotAdded_AndHasIgnoredAttribute_ShouldIncludeItInTheList()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var type = typeof(IgnoreOtherTestObject);
		// Act
		var result = sut.IsIgnored(type);

		// Assert
		result.ShouldBeTrue();

		var ignoredTypesField = typeof(IgnoreSettings).GetRuntimeFields().SingleOrDefault(item => item.Name == _ignoredTypeFieldName);

		if (ignoredTypesField?.GetValue(sut) is ConcurrentDictionary<Type, bool> ignoredTypeArray)
		{
			ignoredTypeArray.ContainsKey(type).ShouldBeTrue();
		}
		else
		{
			Assert.True(false, "The test cannot find the field: " + _ignoredTypeFieldName);
		}
	}

	[Fact(DisplayName = "When the ignored settings does not have the property in the ignored list and there is an [Ignore] attribute." +
						" It should add it on the spot.")]
	public void WhenThePropertyIsNotAdded_AndHasIgnoredAttribute_ShouldIncludeItInTheList()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var propertyInfo = typeof(IgnoreTestObject).GetProperty(nameof(IgnoreTestObject.FirstProperty));

		if (propertyInfo is null)
		{
			Assert.True(false, "The test cannot find the property: " + nameof(IgnoreTestObject.FirstProperty));
		}

		// Act
		var result = sut.IsIgnored(propertyInfo);

		// Assert
		result.ShouldBeTrue();

		var ignoredPropertyInfo = typeof(IgnoreSettings).GetRuntimeFields().SingleOrDefault(item => item.Name == _ignoredPropertyFieldName);

		if (ignoredPropertyInfo?.GetValue(sut) is ConcurrentDictionary<PropertyInfo, bool> ignoredProperties)
		{
			ignoredProperties.ContainsKey(propertyInfo).ShouldBeTrue();
		}
		else
		{
			Assert.True(false, "The test cannot find the field: " + _ignoredPropertyFieldName);
		}
	}


	[Fact]
	public void WhenThereAreIgnoredTypesAndProperties_AndCallClear_ShouldClear()
	{
		// Arrange
		var sut = new IgnoreSettings()
			.Ignore<TestObject>()
			.Ignore<OtherTestObject, string>(x => x.OtherFirstProperty)
			.Ignore<IgnoreOtherTestObject>();

		var ignoredPropertyInfo = typeof(IgnoreSettings).GetRuntimeFields().SingleOrDefault(item => item.Name == _ignoredPropertyFieldName);
		var ignoredTypesField = typeof(IgnoreSettings).GetRuntimeFields().SingleOrDefault(item => item.Name == _ignoredTypeFieldName);

		// Act
		sut.Clear();

		// Assert

		if (ignoredTypesField?.GetValue(sut) is ConcurrentDictionary<Type, bool> ignoredTypesArray)
		{
			ignoredTypesArray.ShouldBeEmpty();
		}
		else
		{
			Assert.True(false, "The test cannot find the field: " + _ignoredTypeFieldName);
		}

		if (ignoredPropertyInfo?.GetValue(sut) is ConcurrentDictionary<PropertyInfo, bool> ignoredProperties)
		{
			ignoredProperties.ShouldBeEmpty();
		}
		else
		{
			Assert.True(false, "The test cannot find the field: " + _ignoredPropertyFieldName);
		}
	}

	[Fact]
	public void WhenTheIgnoreExpressionIsNull_ShouldThrow()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var propertyToIgnore = typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty));

		// Act
		var result = Record.Exception(() => sut.Ignore<TestObject, string>(null));

		// Assert
		result.ShouldBeOfType<ArgumentException>()
			.ParamName.ShouldBe("expression");
		result.Message.ShouldStartWith(Resources.Errors.Skip_RequiresMemberExpression);
	}

	[Fact]
	public void WhenTheIgnoreExpressionIsNotAMemberExpression_ShouldThrow()
	{
		// Arrange
		var sut = new IgnoreSettings();
		var propertyToIgnore = typeof(TestObject).GetProperty(nameof(TestObject.FirstProperty));

		// Act
		var result = Record.Exception(() => sut.Ignore<TestObject, string>(x => x.ToString()));

		// Assert
		result.ShouldBeOfType<ArgumentException>()
			.ParamName.ShouldBe("expression");
		result.Message.ShouldStartWith(Resources.Errors.Skip_RequiresMemberExpression);
	}
}