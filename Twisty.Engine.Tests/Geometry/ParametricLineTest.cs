using System;
using Twisty.Engine.Geometry;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	public class ParametricLineTest
	{
		#region Test Data

		//(Cartesian3dCoordinate p1, Cartesian3dCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double, double, double, double, double, double> ParametricLinesFromPoints = new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double, double, double, double, double, double>()
		{
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(4.0, 5.0, 6.0), 0.0, 4.0, 0.0, 5.0, 0.0, 6.0},
			{new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(4.0, 5.0, 6.0), 1.0, 3.0, 1.0, 4.0, 1.0, 5.0},
			{new Cartesian3dCoordinate(1.0, 2.0, 3.0), new Cartesian3dCoordinate(4.0, 5.0, 6.0), 1.0, 3.0, 2.0, 3.0, 3.0, 3.0},
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
			void a() => new ParametricLine(pointCoordinates);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[InlineData("(0.0 0.0 0.0 4.0 5.0 6.0)", 0.0, 4.0, 0.0, 5.0, 0.0, 6.0)]
		[InlineData("(1.0 1.0 1.0 4.0 5.0 6.0)", 1.0, 4.0, 1.0, 5.0, 1.0, 6.0)]
		[InlineData("(1.0 2.0 3.0 4.0 5.0 6.0)", 1.0, 4.0, 2.0, 5.0, 3.0, 6.0)]
		public void ParametricLine_CreateFromString_BeExpected(string coordinates, double x, double xt, double y, double yt, double z, double zt)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			ParametricLine o = new ParametricLine(coordinates);

			// 3. Verify
			Assert.Equal(x, o.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(xt, o.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(y, o.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(yt, o.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(z, o.Z, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(zt, o.C, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(ParametricLineTest.ParametricLinesFromPoints), MemberType = typeof(ParametricLineTest))]
		public void ParametricLine_CreateFromTwoPoints_BeExpected(Cartesian3dCoordinate p1, Cartesian3dCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			ParametricLine o = ParametricLine.FromTwoPoints(p1, p2);

			// 3. Verify
			Assert.Equal(x, o.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(xt, o.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(y, o.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(yt, o.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(z, o.Z, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(zt, o.C, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		// Parallels lines
		[InlineData("(1.0 0.0 0.0 20.0)", "(0.0 1.0 1.0)", "(0.0 -1.0 -1.0)", true)]
		[InlineData("(0.0 5.5 0.0 18.9)", "(2.0 0.0 0.0)", "(-2.0 0.0 0.0)", true)]
		// Non-Parallels lines
		[InlineData("(1.0 0.0 0.0 20.0)", "(0.0 0.0 0.0)", "(4.0 5.0 6.0)", false)]
		[InlineData("(17.34 8.45 13.78 780.9)", "(17.0 0.55 40.301)", "(14.0 -5.0 -76.8)", false)]
		public void ParametricLine_IsParallelToPlane_BeExpected(string planeCoordinates, string ccCoordinate1, string ccCoordinate2, bool result)
		{
			// 1. Prepare
			Plane p = new Plane(planeCoordinates);
			Cartesian3dCoordinate p1 = new Cartesian3dCoordinate(ccCoordinate1);
			Cartesian3dCoordinate p2 = new Cartesian3dCoordinate(ccCoordinate2);
			ParametricLine line = ParametricLine.FromTwoPoints(p1, p2);

			// 2. Execute
			bool b = line.IsParallelTo(p);

			// 3. Verify
			Assert.Equal(result, b);
		}

		[Theory]
		// Same lines
		[InlineData("(1 0 0 1 1 1)", "(-1 0 0 1 1 1)", true)]
		[InlineData("(1 0 0 1 1 1)", "(-1 0 0 -1 -1 -1)", true)]
		// Parallels lines
		[InlineData("(1 0 0 17 2 3)", "(-1 27 33.5 17 2 3)", true)]
		[InlineData("(1 0 0 10 2 4)", "(9 0 22 20 4 8)", true)]
		// Non-Parallels lines
		[InlineData("(1 0 0 17 2 3)", "(-1 27 33.5 17 2 3.1)", false)]
		[InlineData("(1 0 0 17 2 3)", "(-1 27 33.5 -17 2 3)", false)]
		public void ParametricLine_IsParallelToLine_BeExpected(string line1Coordinates, string line2Coordinates, bool expected)
		{
			// 1. Prepare
			ParametricLine line1 = new ParametricLine(line1Coordinates);
			ParametricLine line2 = new ParametricLine(line2Coordinates);

			// 2. Execute
			bool b1 = line1.IsParallelTo(line2);
			bool b2 = line2.IsParallelTo(line1);

			// 3. Verify
			Assert.Equal(expected, b1);
			Assert.Equal(expected, b2);
		}

		[Theory]
		[InlineData("(1 1 1 2 0 0)", "(1 2 1)", 1)]
		[InlineData("(1.32 2.7 3.4 5.6 7.8 11.4)", "(0.2 0.5 2.3)", 1.2164857559899611)]
		public void ParametricLine_GetDistanceTo_BeExpected(string lineCoordiantes, string ccCoordinate, double expected)
		{
			// 1. Prepare
			Cartesian3dCoordinate p = new Cartesian3dCoordinate(ccCoordinate);
			ParametricLine line = new ParametricLine(lineCoordiantes);

			// 2. Execute
			double r = line.GetDistanceTo(p);

			// 3. Verify
			Assert.Equal(expected, r, GeometryAssert.PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}
