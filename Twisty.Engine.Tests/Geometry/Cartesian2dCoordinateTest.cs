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
		[InlineData("(0 0)", 0, 0)]
		[InlineData("(1.2 0)", 1.2, 0)]
		[InlineData("(0 1.3)", 0, 1.3)]
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

		[Theory]
		[InlineData("(0 0)", "(0 0)", "(0 0)")]
		[InlineData("(0 0)", "(10 0)", "(10 0)")]
		[InlineData("(1 0)", "(-1 0)", "(0 0)")]
		[InlineData("(0 1)", "(0 -1)", "(0 0)")]
		[InlineData("(0 1)", "(1.0  0)", "(1 1)")]
		[InlineData("(1 1)", "(-1 0)", "(0 1)")]
		public void Cartesian2dCoordinate_AddVector_BeExpected(string sourceCc, string addedCc, string expectedCc)
		{
			// 1. Prepare
			Cartesian2dCoordinate source = new Cartesian2dCoordinate(sourceCc);
			Cartesian2dCoordinate added = new Cartesian2dCoordinate(addedCc);
			Cartesian2dCoordinate expected = new Cartesian2dCoordinate(expectedCc);

			// 2. Execute
			Cartesian2dCoordinate r1 = source + added;
			Cartesian2dCoordinate r2 = added + source;

			// 3. Verify
			Assert.Equal(expected.X, r1.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r1.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.X, r2.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r2.Y, PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(0 0)", "(0 0)", "(0 0)")]
		[InlineData("(0 0)", "(10 0)", "(-10 0)")]
		[InlineData("(1 0)", "(-1 0)", "(2 0)")]
		[InlineData("(0 1)", "(0 -1)", "(0 2)")]
		[InlineData("(0 0)", "(1 0)", "(-1 0)")]
		public void Cartesian2dCoordinate_SubstractVector_BeExpected(string sourceCc, string substractedCc, string expectedCc)
		{
			// 1. Prepare
			Cartesian2dCoordinate source = new Cartesian2dCoordinate(sourceCc);
			Cartesian2dCoordinate substracted = new Cartesian2dCoordinate(substractedCc);
			Cartesian2dCoordinate expected = new Cartesian2dCoordinate(expectedCc);

			// 2. Execute
			Cartesian2dCoordinate r = source - substracted;

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}