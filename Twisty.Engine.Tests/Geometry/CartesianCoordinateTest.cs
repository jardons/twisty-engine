using System;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CartesianCoordinateTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, CartesianCoordinate> AddVector
			= new TheoryData<CartesianCoordinate, CartesianCoordinate, CartesianCoordinate>()
		{
			{new CartesianCoordinate(0.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 0.0, 0.0), new CartesianCoordinate(10.0, 0.0, 0.0), new CartesianCoordinate(10.0, 0.0, 0.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(-1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), new CartesianCoordinate(0.0, -1.0, 0.0), new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 0.0, 1.0), new CartesianCoordinate(0.0, 0.0, -1.0), new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 0.0, 1.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 1.0)},
			{new CartesianCoordinate(1.0, 0.0, 1.0), new CartesianCoordinate(-1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<CartesianCoordinate, double, CartesianCoordinate> RotateAll
			= new TheoryData<CartesianCoordinate, double, CartesianCoordinate>()
		{
			{new CartesianCoordinate(0.0, 0.0, 0.0), 0.0, new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), 0.0, new CartesianCoordinate(1.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), 0.0, new CartesianCoordinate(0.0, 1.0, 0.0)},
			{new CartesianCoordinate(0.0, 0.0, 1.0), 0.0, new CartesianCoordinate(0.0, 0.0, 1.0)},
			{new CartesianCoordinate(0.0, 0.0, 0.0), Math.PI * 2.0, new CartesianCoordinate(0.0, 0.0, 0.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI * 2.0, new CartesianCoordinate(1.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0, new CartesianCoordinate(0.0, 1.0, 0.0)},
			{new CartesianCoordinate(0.0, 0.0, 1.0), Math.PI * 2.0, new CartesianCoordinate(0.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<CartesianCoordinate, double, CartesianCoordinate> RotateAroundX
			= new TheoryData<CartesianCoordinate, double, CartesianCoordinate>()
		{
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new CartesianCoordinate(0.0, 0.0, 1.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI, new CartesianCoordinate(0.0, -1.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new CartesianCoordinate(0.0, 0.0, -1.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 3.0, new CartesianCoordinate(0.0, -0.5, 0.866025403784439)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 360.0 * 173.0, new CartesianCoordinate(0.0, -0.992546151641322, 0.121869343405148)},
			{new CartesianCoordinate(0.0, 1.0, 3.0), Math.PI * 2.0 / 360.0 * 173.0, new CartesianCoordinate(0.0, -1.35815418185676, -2.85576911151882)},
		};

		public static readonly TheoryData<CartesianCoordinate, double, CartesianCoordinate> RotateAroundY
			= new TheoryData<CartesianCoordinate, double, CartesianCoordinate>()
		{
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0, new CartesianCoordinate(0.0, 0.0, -1.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI, new CartesianCoordinate(-1.0, 0.0, 0.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0 * 3.0, new CartesianCoordinate(0.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<CartesianCoordinate, double, CartesianCoordinate> RotateAroundZ
			= new TheoryData<CartesianCoordinate, double, CartesianCoordinate>()
		{
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new CartesianCoordinate(-1.0, 0.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI, new CartesianCoordinate(0.0, -1.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new CartesianCoordinate(1.0, 0.0, 0.0)},
		};

		public static readonly TheoryData<CartesianCoordinate, double, CartesianCoordinate, CartesianCoordinate> RotateAroundVector
			= new TheoryData<CartesianCoordinate, double, CartesianCoordinate, CartesianCoordinate>()
		{
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 1.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, -1.0, 0.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, -1.0)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 3.0, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, -0.5, 0.866025403784439)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 360.0 * 173.0, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, -0.992546151641322, 0.121869343405148)},
			{new CartesianCoordinate(0.0, 1.0, 3.0), Math.PI * 2.0 / 360.0 * 173.0, new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, -1.35815418185676, -2.85576911151882)},
			{new CartesianCoordinate(0.0, 1.0, 0.0), Math.PI, new CartesianCoordinate(2.0, 0.0, 0.0), new CartesianCoordinate(0.0, -1.0, 0.0)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0, new CartesianCoordinate(1.0, 1.0, 1.0), new CartesianCoordinate(0.3333333333, 0.910683602522959, -0.244016935856292)},
			{new CartesianCoordinate(1.0, 0.0, 0.0), Math.PI, new CartesianCoordinate(1.0, 1.0, 1.0), new CartesianCoordinate(-0.3333333333, 0.66666666666, 0.66666666666)},
			{new CartesianCoordinate(6.0, 5.0, 8.0), Math.PI / 2.0, new CartesianCoordinate(3.0, 7.0, 4.0), new CartesianCoordinate(7.63086094072344, 8.04054054054054, 1.45590834851147)},
			{new CartesianCoordinate(1.0, 5.0, 8.0), 100.0, new CartesianCoordinate(3.0, 7.0, 9.0), new CartesianCoordinate(0.7167446324, 5.71852897500143, 7.53556258643274)},
		};

		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, double> CalculateTheta
			= new TheoryData<CartesianCoordinate, CartesianCoordinate, double>()
		{
			{new CartesianCoordinate(0.0, 1.0, 0.0), new CartesianCoordinate(-1.0, 0.0, 0.0), Math.PI / 2.0},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.AddVector), MemberType = typeof(CartesianCoordinateTest))]
		public void CartesianCoordinate_AddVector_BeExpected(CartesianCoordinate source, CartesianCoordinate added, CartesianCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			CartesianCoordinate r1 = source + added;
			CartesianCoordinate r2 = added + source;

			// 3. Verify
			Assert.Equal(expected.X, r1.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r1.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r1.Z, PRECISION_DOUBLE);
			Assert.Equal(expected.X, r2.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r2.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r2.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.RotateAll), MemberType = typeof(CartesianCoordinateTest))]
		[MemberData(nameof(CartesianCoordinateTest.RotateAroundX), MemberType = typeof(CartesianCoordinateTest))]
		public void CartesianCoordinate_RotateAroundX_BeExpected(CartesianCoordinate source, double theta, CartesianCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			CartesianCoordinate r = source.RotateAroundX(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.RotateAll), MemberType = typeof(CartesianCoordinateTest))]
		[MemberData(nameof(CartesianCoordinateTest.RotateAroundY), MemberType = typeof(CartesianCoordinateTest))]
		public void CartesianCoordinate_RotateAroundY_BeExpected(CartesianCoordinate source, double theta, CartesianCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			CartesianCoordinate r = source.RotateAroundY(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.RotateAll), MemberType = typeof(CartesianCoordinateTest))]
		[MemberData(nameof(CartesianCoordinateTest.RotateAroundZ), MemberType = typeof(CartesianCoordinateTest))]
		public void CartesianCoordinate_RotateAroundZ_BeExpected(CartesianCoordinate source, double theta, CartesianCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			CartesianCoordinate r = source.RotateAroundZ(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.RotateAroundVector), MemberType = typeof(CartesianCoordinateTest))]
		public void CartesianCoordinate_RotateAroundVector_BeExpected(CartesianCoordinate source, double theta, CartesianCoordinate k, CartesianCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			CartesianCoordinate r = source.RotateAroundVector(k, theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CartesianCoordinateTest.CalculateTheta), MemberType = typeof(CartesianCoordinateTest))]
		public void CatesianCoordinate_CalculateThetaBetweenVector_ReturnExpected(CartesianCoordinate c1, CartesianCoordinate c2, double expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			double r1 = c1.GetThetaTo(c2);
			double r2 = c2.GetThetaTo(c1);

			// 3. Verify
			Assert.Equal(expected, r1, PRECISION_DOUBLE);
			Assert.Equal(expected, r2, PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}