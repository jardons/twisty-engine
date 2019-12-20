using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class Cartesian2dCoordinateTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Methods

		[Theory]
		[InlineData("(0.0 0.0)", 0.0, 0.0)]
		[InlineData("(1.2 0.0)", 1.2, 0.0)]
		[InlineData("(0.0 1.3)", 0.0, 1.3)]
		[InlineData("(6.45 -17.9)", 6.45, -17.9)]
		public void Cartesian2dCoordinate_CreateFromString_BeExpected(string pointCoordinates, double expectedX, double expectedY)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Cartesian2dCoordinate c = new Cartesian2dCoordinate(pointCoordinates);

			// 3. Verify
			Assert.Equal(expectedX, c.X, PRECISION_DOUBLE);
			Assert.Equal(expectedY, c.Y, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.InvalidCoordinates), MemberType = typeof(CoordinateConverterTest))]
		[InlineData("(1)")]
		[InlineData("(1 2 3)")]
		public void Cartesian2dCoordinate_CreateFromInvalidString_ThrowArgumentException(string pointCoordinates)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Action a = () => new Cartesian2dCoordinate(pointCoordinates);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		#endregion Test Methods
	}
}