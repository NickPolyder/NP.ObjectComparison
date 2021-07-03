using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ObjectComparison.Results;
using Shouldly;
using Xunit;

namespace ObjectComparison.Tests.Results
{
	public class ObjectItemExtensionsTests
	{
		private IFixture _fixture;

		public ObjectItemExtensionsTests()
		{
			_fixture = new Fixture();
		}
		
		[Fact]
		public void HasChanges_WhenEnumerableIsNull_ShouldReturnFalse()
		{
			// Arrange
			List<ObjectItem> sut = null;

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeFalse();
		}
		
		[Fact]
		public void HasChanges_WhenItHasNone_ShouldReturnFalse()
		{
			// Arrange
			var sut = new List<ObjectItem>();
			foreach (var i in Enumerable.Range(0, 10))
			{
				var builder = new ObjectItem.Builder()
					.SetName(_fixture.Create<string>());

				sut.Add(builder.Build());
			}

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void HasChanges_WhenItHasAtLeastOne_ShouldReturnTrue()
		{
			// Arrange
			var sut = new List<ObjectItem>();
			foreach (var i in Enumerable.Range(0,10))
			{
				var builder = new ObjectItem.Builder()
					.SetName(_fixture.Create<string>());
				
				sut.Add(builder.Build());
			}
			var anotherBuilder = new ObjectItem.Builder()
				.SetName(_fixture.Create<string>())
				.HasChanges();

			sut.Add(anotherBuilder.Build());

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeTrue();
		}
	}
}