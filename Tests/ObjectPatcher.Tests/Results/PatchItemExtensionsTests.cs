using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ObjectPatcher.Results;
using Shouldly;
using Xunit;

namespace ObjectPatcher.Tests.Results
{
	public class PatchItemExtensionsTests
	{
		private IFixture _fixture;

		public PatchItemExtensionsTests()
		{
			_fixture = new Fixture();
		}
		
		[Fact]
		public void HasChanges_WhenEnumerableIsNull_ShouldReturnFalse()
		{
			// Arrange
			List<PatchItem> sut = null;

			// Act
			var result = sut.HasChanges();

			// Assert
			result.ShouldBeFalse();
		}
		
		[Fact]
		public void HasChanges_WhenItHasNone_ShouldReturnFalse()
		{
			// Arrange
			var sut = new List<PatchItem>();
			foreach (var i in Enumerable.Range(0, 10))
			{
				var builder = new PatchItem.Builder()
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
			var sut = new List<PatchItem>();
			foreach (var i in Enumerable.Range(0,10))
			{
				var builder = new PatchItem.Builder()
					.SetName(_fixture.Create<string>());
				
				sut.Add(builder.Build());
			}
			var anotherBuilder = new PatchItem.Builder()
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