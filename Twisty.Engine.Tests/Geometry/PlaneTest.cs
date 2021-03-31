using System;
using Twisty.Engine.Geometry;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class PlaneTest
	{
		#region Test Data

		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double, double, double, double> CreationFromNormal
			= new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double, double, double, double>()
		{
			{ new Cartesian3dCoordinate(2.0, 3.0, 4.0), new Cartesian3dCoordinate(1.0, 2.0, 3.0), 2.0, 3.0, 4.0, -20.0 }
		};

		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate, bool> IsAbovePlane
			= new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate, bool>()
		{
			{ new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), false },
			{ new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), false },
			{ new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), true },
			{ new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), false },
			{ new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.3, 0.3, 0.3), false },
			{ new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(1.0, 1.0, 0.0), true },
			{ new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(1.0, 1.0, 2.0), true },
			{ new Cartesian3dCoordinate(2.0, 3.0, 4.0), new Cartesian3dCoordinate(1.0, 2.0, 3.0), new Cartesian3dCoordinate(1.0, 2.0, 3.0), false }
		};

		public static readonly TheoryData<string, double, double, double, double> CreationFromString = new TheoryData<string, double, double, double, double>()
		{
			{ "(2 3 4 -20)", 2.0, 3.0, 4.0, -20.0 },
			{ "(1 -1 2 3)", 1.0, -1.0, 2.0, 3.0 },
			{ "(1.5 -1.5 2.2 3.3)", 1.5, -1.5, 2.2, 3.3 },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromString), MemberType = typeof(PlaneTest))]
		public void Plane_CreateFromString_BeExpected(string planCoordinates, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Plane p = new Plane(planCoordinates);

			// 3. Verify
			Assert.Equal(expectedA, p.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedB, p.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedC, p.C, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedD, p.D, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.InvalidCoordinates), MemberType = typeof(CoordinateConverterTest))]
		[InlineData("(1)")]
		[InlineData("(1 2)")]
		[InlineData("(1 2 3)")]
		[InlineData("(1 2 3 4 5)")]
		public void Plane_CreateFromInvalidString_ThrowArgumentException(string planeCoordinates)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			void a() => new Plane(planeCoordinates);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromString), MemberType = typeof(PlaneTest))]
		public void Plane_CreateFromDoubles_BeExpected(string planeCoordinates, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Plane p = new Plane(expectedA, expectedB, expectedC, expectedD);

			// 3. Verify
			Assert.Equal(expectedA, p.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedB, p.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedC, p.C, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedD, p.D, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromNormal), MemberType = typeof(PlaneTest))]
		public void Plane_CreateFromNormalAndPoint_BeExpected(Cartesian3dCoordinate normal, Cartesian3dCoordinate point, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Plane p = new Plane(normal, point);

			// 3. Verify
			Assert.Equal(expectedA, p.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedB, p.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedC, p.C, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedD, p.D, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromNormal), MemberType = typeof(PlaneTest))]
		public void Plane_CheckCreationPointIsOnPlane_True(Cartesian3dCoordinate normal, Cartesian3dCoordinate point, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			Plane p = new Plane(normal, point);

			// 2. Execute
			bool isOnPlane = p.IsOnPlane(point);

			// 3. Verify
			Assert.True(isOnPlane);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.IsAbovePlane), MemberType = typeof(PlaneTest))]
		public void Plane_CheckPointIsAbovePlane_Expected(Cartesian3dCoordinate planeNormal, Cartesian3dCoordinate planePoint, Cartesian3dCoordinate point, bool expected)
		{
			// 1. Prepare
			Plane p = new Plane(planeNormal, planePoint);

			// 2. Execute
			bool isAbovePlane = p.IsAbovePlane(point);

			// 3. Verify
			Assert.Equal(expected, isAbovePlane);
		}

		[Theory]
		[InlineData("(1 0 0 0)", "(-1 0 0)", true)]
		[InlineData("(1 0 0 0)", "(0 0 0)", false)]
		[InlineData("(1 0 0 0)", "(1 0 0)", false)]
		public void Plane_CheckPointIsBelowPlane_Expected(string planeCoordinate, string pointCoordinate, bool expected)
		{
			// 1. Prepare
			Plane p = new Plane(planeCoordinate);
			Cartesian3dCoordinate point = new Cartesian3dCoordinate(pointCoordinate);

			// 2. Execute
			bool r = p.IsBelowPlane(point);

			// 3. Verify
			Assert.Equal(expected, r);
		}

		[Theory]
		[InlineData("(2 3 4 -20)", "(-2 0 0 6 5 6)", "(0.823529411764706 2.35294117647059 2.82352941176471)")]
		[InlineData("(0 0 1 -1)", "(-0.5 -0.5 0.71 0 0 1)", "(-0.5 -0.5 1)")]
		public void Plane_GetIntersectionFromLine_Expected(string planeCoordinate, string lineCoordinate, string expectedCoordinate)
		{
			// 1. Prepare
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCoordinate);
			ParametricLine line = new ParametricLine(lineCoordinate);
			Plane p = new Plane(planeCoordinate);

			// 2. Execute
			Cartesian3dCoordinate r = p.GetIntersection(line);

			// 3. Verify
			Assert.Equal(expected.X, r.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(1 0 0 -29)", "(1 0 0 -29)", true)]
		[InlineData("(2 3 4 -29)", "(2 3 4 -27)", true)]
		[InlineData("(2 3 4 -29)", "(2 3 4 29)", true)]
		[InlineData("(2 3 4 -29)", "(4 3 2 29)", false)]
		public void Plane_IsParallelTo_Expected(string firstPlaneCoordinate, string secondPlaneCoordinate, bool expected)
		{
			// 1. Prepare
			Plane p1 = new Plane(firstPlaneCoordinate);
			Plane p2 = new Plane(secondPlaneCoordinate);

			// 2. Execute
			bool r1 = p1.IsParallelTo(p2);
			bool r2 = p2.IsParallelTo(p1);

			// 3. Verify
			Assert.Equal(expected, r1);
			Assert.Equal(expected, r2);
		}

		[Theory]
		[InlineData("(2 3 4 -29)", "(2 3 4)", 0.0)]
		[InlineData("(1 0 0 -1)", "(2 0 0)", 1.0)]
		[InlineData("(1 0 0 -1)", "(3 0 0)", 2.0)]
		[InlineData("(1 0 0 -1)", "(3.5 0 0)", 2.5)]
		[InlineData("(1 0 0 -1)", "(0 0 0)", 1.0)]
		[InlineData("(1 0 0 -1)", "(2 1 3)", 1.0)]
		public void Plane_GetDistanceToPoint_Expected(string planeCc, string pointCc, double distance)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(pointCc);
			Plane p = new Plane(planeCc);

			// 2. Execute
			double r = p.GetDistanceTo(cc);

			// 3. Verify
			Assert.Equal(distance, r, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(2 3 4 -29)", "(2 3 4)", "(2 3 4)")]
		[InlineData("(2 3 4 -29)", "(4 6 8)", "(2 3 4)")]
		[InlineData("(2 3 4 -29)", "(6 9 12)", "(2 3 4)")]
		[InlineData("(2 3 4 -29)", "(1 1.5 2)", "(2 3 4)")]
		public void Plane_GetIntersectionFromCartesian3dCoordinate_Expected(string planeCoordinate, string Cartesian3dCoordinate, string expectedCoordinate)
		{
			// 1. Prepare
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCoordinate);
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(Cartesian3dCoordinate);
			Plane p = new Plane(planeCoordinate);

			// 2. Execute
			Cartesian3dCoordinate r = p.GetIntersection(cc);

			// 3. Verify
			Assert.Equal(expected.X, r.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(1 0 0 -29)", "(5 0 0)", "(5 0 0 1 0 0)")]
		[InlineData("(2 3 4 -29)", "(4 6 8)", "(4 6 8 2 3 4)")]
		[InlineData("(2 3 4 -29)", "(6 9 12)", "(6 9 12 2 3 4)")]
		[InlineData("(2 3 4 -29)", "(1 1.5 2)", "(1 1.5 2 2 3 4)")]
		public void Plane_GetPerpendicular_Expected(string planeCoordinate, string Cartesian3dCoordinate, string expectedCoordinate)
		{
			// 1. Prepare
			ParametricLine expected = new ParametricLine(expectedCoordinate);
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(Cartesian3dCoordinate);
			Plane p = new Plane(planeCoordinate);

			// 2. Execute
			ParametricLine l = p.GetPerpendicular(cc);

			// 3. Verify
			Assert.Equal(expected.X, l.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Y, l.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Z, l.Z, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.A, l.A, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.B, l.B, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.C, l.C, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(-1 0 0 0)", "(0 0 -1 0)", "(0 0 0 0 -1 0)")]
		[InlineData("(-1 0 0 0)", "(0 0 -1 1)", "(0 0 1 0 -1 0)")]
		[InlineData("(0 -1 0 0)", "(0 0 -1 1)", "(0 0 1 1 0 0)")]
		[InlineData("(-1 0 0 0)", "(0 1 0 -1)", "(0 1 0 0 0 -1)")]
		[InlineData("(4 3 2 1)", "(1 2 3 4)", "(2 -3 0 1 -2 1)")]
		[InlineData("(7 8 7 8)", "(8 7 8 7)", "(0 -1 0 1 0 -1)")]
		[InlineData("(50 20 40 60)", "(2 4 8 16)", "(0.5 -4.25 0 0 -2 1)")]
		public void Plane_GetIntersectionFromPlaneAndCheckResultProperties_Expected(string planeCoordinate, string secondPlaneCoordinate, string expectedCoordinate)
		{
			// 1. Prepare
			ParametricLine expected = new ParametricLine(expectedCoordinate);
			Cartesian3dCoordinate expectedVector = expected.Vector.Normalize();
			Plane p1 = new Plane(planeCoordinate);
			Plane p2 = new Plane(secondPlaneCoordinate);

			// 2. Execute
			ParametricLine l = p1.GetIntersection(p2);
			Cartesian3dCoordinate vector = l.Vector.Normalize();

			// Warning ! thoses tests are dependent of other methods, in case of errors, checks the UnitsTests for those methods used for validation.
			bool isOnP1 = p1.IsOnPlane(l.Point);
			bool isOnP2 = p2.IsOnPlane(l.Point);
			bool isFurtherOnP1 = p1.IsOnPlane(l.Point + l.Vector);
			bool isFurtherOnP2 = p2.IsOnPlane(l.Point + l.Vector);
			bool isParrallelToP1 = new ParametricLine(l.Vector).IsParallelTo(p1);
			bool isParrallelToP2 = new ParametricLine(l.Vector).IsParallelTo(p2);

			// 3. Verify
			Assert.True(isOnP1);
			Assert.True(isOnP2);
			Assert.True(isFurtherOnP1);
			Assert.True(isFurtherOnP2);
			Assert.True(isParrallelToP1);
			Assert.True(isParrallelToP2);
			Assert.Equal(expected.X, l.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Y, l.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Z, l.Z, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedVector.X, vector.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedVector.Y, vector.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedVector.Z, vector.Z, GeometryAssert.PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(1 0 0 0)", "(1 1 0)", "(0 1 0)")]
		[InlineData("(1 0 0 0)", "(1 1 1)", "(0 1 1)")]
		[InlineData("(1 0 0 -10)", "(1 1 0)", "(0 1 0)")]
		public void Plane_GetVectorProjection_Expected(string planeCoordinate, string vectorCoordinates, string expectedCoordinate)
		{
			// 1. Prepare
			Plane p = new Plane(planeCoordinate);
			Cartesian3dCoordinate v = new Cartesian3dCoordinate(vectorCoordinates);
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCoordinate);

			// 2. Execute
			var r = p.GetVectorProjection(v);

			// 3. Verify
			GeometryAssert.SameVector(expected, r);
		}
		
		#endregion Test Methods
	}
}