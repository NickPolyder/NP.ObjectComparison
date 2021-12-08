using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NP.ObjectComparison.Results;
using Shouldly;
using Xunit;

namespace NP.ObjectComparison.Tests.Results
{
	public class DiffSnapshotExtensionsTests
	{
		private IFixture _fixture;

		public DiffSnapshotExtensionsTests()
		{
			_fixture = new Fixture();
		}
		
		[Fact]
		public void HasChanges_WhenEnumerableIsNull_ShouldReturnFalse()
		{
			// Arrange
			List<DiffSnapshot> sut = null;

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeFalse();
		}
		
		[Fact]
		public void HasChanges_WhenItHasNone_ShouldReturnFalse()
		{
			// Arrange
			var sut = new List<DiffSnapshot>();
			foreach (var i in Enumerable.Range(0, 10))
			{
				var builder = new DiffSnapshot.Builder()
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
			var sut = new List<DiffSnapshot>();
			foreach (var i in Enumerable.Range(0,10))
			{
				var builder = new DiffSnapshot.Builder()
					.SetName(_fixture.Create<string>());
				
				sut.Add(builder.Build());
			}
			var anotherBuilder = new DiffSnapshot.Builder()
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