using System;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	public class ParametricLineTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		//(CartesianCoordinate p1, CartesianCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double, double, double> ParametricLinesFromPoints = new TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double, double, double>()
		{
			{new CartesianCoordinate(0.0, 0.0, 0.0), new CartesianCoordinate(4.0, 5.0, 6.0), 0.0, 4.0, 0.0, 5.0, 0.0, 6.0},
			{new CartesianCoordinate(1.0, 1.0, 1.0), new CartesianCoordinate(4.0, 5.0, 6.0), 1.0, 3.0, 1.0, 4.0, 1.0, 5.0},
			{new CartesianCoordinate(1.0, 2.0, 3.0), new CartesianCoordinate(4.0, 5.0, 6.0), 1.0, 3.0, 2.0, 3.0, 3.0, 3.0},
		};

		//(string coordinates, double x, double xt, double y, double yt, double z, double zt)
		public static readonly TheoryData<string, double, double, double, double, double, double> ParametricLinesFromString = new TheoryData<string, double, double, double, double, double, double>()
		{
			{ "(0.0 0.0 0.0 4.0 5.0 6.0)", 0.0, 4.0, 0.0, 5.0, 0.0, 6.0 },
			{ "(1.0 1.0 1.0 4.0 5.0 6.0)", 1.0, 4.0, 1.0, 5.0, 1.0, 6.0 },
			{ "(1.0 2.0 3.0 4.0 5.0 6.0)", 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 },
		};

		//(string planeCoordinates, string ccCoordinate1, string ccCoordinate2, bool result)
		public static readonly TheoryData<string, string, string, bool> ParametricLinesAndPlanes = new TheoryData<string, string, string, bool>()
		{
			// Parallels lines
			{ "(1.0 0.0 0.0 20.0)", "(0.0 1.0 1.0)", "(0.0 -1.0 -1.0)", true },
			{ "(0.0 5.5 0.0 18.9)", "(2.0 0.0 0.0)", "(-2.0 0.0 0.0)", true },
			// Non-Parallels lines
			{ "(1.0 0.0 0.0 20.0)", "(0.0 0.0 0.0)", "(4.0 5.0 6.0)", false },
			{ "(17.34 8.45 13.78 780.9)", "(17.0 0.55 40.301)", "(14.0 -5.0 -76.8)", false },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.InvalidCoordinates), MemberType = typeof(CoordinateConverterTest))]
		[InlineData("(1)")]
		[InlineData("(1 2)")]
		[InlineData("(1 2 3)")]
		[InlineData("(1 2 3 4)")]
		[InlineData("(1 2 3 4 5)")]
		[InlineData("(1 2 3 4 5 6 7)")]
		public void ParametricLine_CreateFromInvalidString_ThrowArgumentException(string pointCoordinates)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Action a = () => new ParametricLine(pointCoordinates);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[MemberData(nameof(ParametricLineTest.ParametricLinesFromString), MemberType = typeof(ParametricLineTest))]
		public void ParametricLine_CreateFromString_BeExpected(string coordinates, double x, double xt, double y, double yt, double z, double zt)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			ParametricLine o = new ParametricLine(coordinates);

			// 3. Verify
			Assert.Equal(x, o.X, PRECISION_DOUBLE);
			Assert.Equal(xt, o.A, PRECISION_DOUBLE);
			Assert.Equal(y, o.Y, PRECISION_DOUBLE);
			Assert.Equal(yt, o.B, PRECISION_DOUBLE);
			Assert.Equal(z, o.Z, PRECISION_DOUBLE);
			Assert.Equal(zt, o.C, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(ParametricLineTest.ParametricLinesFromPoints), MemberType = typeof(ParametricLineTest))]
		public void ParametricLine_CreateFromTwoPoints_BeExpected(CartesianCoordinate p1, CartesianCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			ParametricLine o = ParametricLine.FromTwoPoints(p1, p2);

			// 3. Verify
			Assert.Equal(x, o.X, PRECISION_DOUBLE);
			Assert.Equal(xt, o.A, PRECISION_DOUBLE);
			Assert.Equal(y, o.Y, PRECISION_DOUBLE);
			Assert.Equal(yt, o.B, PRECISION_DOUBLE);
			Assert.Equal(z, o.Z, PRECISION_DOUBLE);
			Assert.Equal(zt, o.C, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(ParametricLineTest.ParametricLinesAndPlanes), MemberType = typeof(ParametricLineTest))]
		public void ParametricLine_IsParallelToPlane_BeExpected(string planeCoordinates, string ccCoordinate1, string ccCoordinate2, bool result)
		{
			// 1. Prepare
			Plane p = new Plane(planeCoordinates);
			CartesianCoordinate p1 = new CartesianCoordinate(ccCoordinate1);
			CartesianCoordinate p2 = new CartesianCoordinate(ccCoordinate2);
			ParametricLine line = ParametricLine.FromTwoPoints(p1, p2);

			// 2. Execute
			bool b = line.IsParallelTo(p);

			// 3. Verify
			Assert.Equal(result, b);
		}

		#endregion Test Methods
	}
}
