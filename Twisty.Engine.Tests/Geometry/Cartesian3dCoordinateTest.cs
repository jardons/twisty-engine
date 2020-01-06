using System;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class Cartesian3dCoordinateTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		//(Cartesian3dCoordinate source, Cartesian3dCoordinate added, Cartesian3dCoordinate expected)
		public static readonly TheoryData<string, double, double, double> CreateFromString
			= new TheoryData<string, double, double, double>()
		{
			{ "(0.0 0.0 0.0)", 0.0, 0.0, 0.0 },
			{ "(1.2 0.0 0.0)", 1.2, 0.0, 0.0 },
			{ "(0.0 1.3 0.0)", 0.0, 1.3, 0.0 },
			{ "(0.0 0.0 1.4)", 0.0, 0.0, 1.4 },
		};

		//(Cartesian3dCoordinate source, Cartesian3dCoordinate added, Cartesian3dCoordinate expected)
		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate> AddVector
			= new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(10.0, 0.0, 0.0), new Cartesian3dCoordinate(10.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), new Cartesian3dCoordinate(0.0, -1.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), new Cartesian3dCoordinate(0.0, 0.0, -1.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(1.0, 0.0, 1.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 1.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
		};

		//(Cartesian3dCoordinate source, Cartesian3dCoordinate substracted, Cartesian3dCoordinate expected)
		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate> SubstractVector
			= new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), new Cartesian3dCoordinate(10.0, 0.0, 0.0), new Cartesian3dCoordinate(-10.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), new Cartesian3dCoordinate(2.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), new Cartesian3dCoordinate(0.0, -1.0, 0.0), new Cartesian3dCoordinate(0.0, 2.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), new Cartesian3dCoordinate(0.0, 0.0, -1.0), new Cartesian3dCoordinate(0.0, 0.0, 2.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 1.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 1.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), new Cartesian3dCoordinate(2.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate> RotateAll
			= new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), 0.0, new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), 0.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), 0.0, new Cartesian3dCoordinate(0.0, 1.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), 0.0, new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 0.0), Math.PI * 2.0, new Cartesian3dCoordinate(0.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI * 2.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0, new Cartesian3dCoordinate(0.0, 1.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 0.0, 1.0), Math.PI * 2.0, new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate> RotateAroundX
			= new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI, new Cartesian3dCoordinate(0.0, -1.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new Cartesian3dCoordinate(0.0, 0.0, -1.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 3.0, new Cartesian3dCoordinate(0.0, -0.5, 0.866025403784439)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 360.0 * 173.0, new Cartesian3dCoordinate(0.0, -0.992546151641322, 0.121869343405148)},
			{new Cartesian3dCoordinate(0.0, 1.0, 3.0), Math.PI * 2.0 / 360.0 * 173.0, new Cartesian3dCoordinate(0.0, -1.35815418185676, -2.85576911151882)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate> RotateAroundY
			= new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0, new Cartesian3dCoordinate(0.0, 0.0, -1.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI, new Cartesian3dCoordinate(-1.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0 * 3.0, new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate> RotateAroundZ
			= new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new Cartesian3dCoordinate(-1.0, 0.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI, new Cartesian3dCoordinate(0.0, -1.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate, Cartesian3dCoordinate> RotateAroundVector
			= new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate, Cartesian3dCoordinate>()
		{
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, 1.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, -1.0, 0.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI / 2.0 * 3.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, 0.0, -1.0)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 3.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, -0.5, 0.866025403784439)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI * 2.0 / 360.0 * 173.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, -0.992546151641322, 0.121869343405148)},
			{new Cartesian3dCoordinate(0.0, 1.0, 3.0), Math.PI * 2.0 / 360.0 * 173.0, new Cartesian3dCoordinate(1.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, -1.35815418185676, -2.85576911151882)},
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), Math.PI, new Cartesian3dCoordinate(2.0, 0.0, 0.0), new Cartesian3dCoordinate(0.0, -1.0, 0.0)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI / 2.0, new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(0.3333333333, 0.910683602522959, -0.244016935856292)},
			{new Cartesian3dCoordinate(1.0, 0.0, 0.0), Math.PI, new Cartesian3dCoordinate(1.0, 1.0, 1.0), new Cartesian3dCoordinate(-0.3333333333, 0.66666666666, 0.66666666666)},
			{new Cartesian3dCoordinate(6.0, 5.0, 8.0), Math.PI / 2.0, new Cartesian3dCoordinate(3.0, 7.0, 4.0), new Cartesian3dCoordinate(7.63086094072344, 8.04054054054054, 1.45590834851147)},
			{new Cartesian3dCoordinate(1.0, 5.0, 8.0), 100.0, new Cartesian3dCoordinate(3.0, 7.0, 9.0), new Cartesian3dCoordinate(0.7167446324, 5.71852897500143, 7.53556258643274)},
		};

		public static readonly TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double> CalculateTheta
			= new TheoryData<Cartesian3dCoordinate, Cartesian3dCoordinate, double>()
		{
			{new Cartesian3dCoordinate(0.0, 1.0, 0.0), new Cartesian3dCoordinate(-1.0, 0.0, 0.0), Math.PI / 2.0},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.CreateFromString), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_CreateFromString_BeExpected(string pointCoordinates, double expectedX, double expectedY, double expectedZ)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Cartesian3dCoordinate c = new Cartesian3dCoordinate(pointCoordinates);

			// 3. Verify
			Assert.Equal(expectedX, c.X, PRECISION_DOUBLE);
			Assert.Equal(expectedY, c.Y, PRECISION_DOUBLE);
			Assert.Equal(expectedZ, c.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.InvalidCoordinates), MemberType = typeof(CoordinateConverterTest))]
		[InlineData("(1)")]
		[InlineData("(1 2)")]
		[InlineData("(1 2 )")]
		[InlineData("(1 2 3 4)")]
		public void Cartesian3dCoordinate_CreateFromInvalidString_ThrowArgumentException(string pointCoordinates)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Action a = () => new Cartesian3dCoordinate(pointCoordinates);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.AddVector), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_AddVector_BeExpected(Cartesian3dCoordinate source, Cartesian3dCoordinate added, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r1 = source + added;
			Cartesian3dCoordinate r2 = added + source;

			// 3. Verify
			Assert.Equal(expected.X, r1.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r1.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r1.Z, PRECISION_DOUBLE);
			Assert.Equal(expected.X, r2.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r2.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r2.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.SubstractVector), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_SubstractVector_BeExpected(Cartesian3dCoordinate source, Cartesian3dCoordinate substracted, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r = source - substracted;

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAll), MemberType = typeof(Cartesian3dCoordinateTest))]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAroundX), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_RotateAroundX_BeExpected(Cartesian3dCoordinate source, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r = source.RotateAroundX(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAll), MemberType = typeof(Cartesian3dCoordinateTest))]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAroundY), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_RotateAroundY_BeExpected(Cartesian3dCoordinate source, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r = source.RotateAroundY(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAll), MemberType = typeof(Cartesian3dCoordinateTest))]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAroundZ), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_RotateAroundZ_BeExpected(Cartesian3dCoordinate source, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r = source.RotateAroundZ(theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.RotateAroundVector), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_RotateAroundVector_BeExpected(Cartesian3dCoordinate source, double theta, Cartesian3dCoordinate k, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			Cartesian3dCoordinate r = source.RotateAroundVector(k, theta);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(Cartesian3dCoordinateTest.CalculateTheta), MemberType = typeof(Cartesian3dCoordinateTest))]
		public void Cartesian3dCoordinate_CalculateThetaBetweenVector_ReturnExpected(Cartesian3dCoordinate c1, Cartesian3dCoordinate c2, double expected)
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

		[Theory]
		// Exceptions to formula
		[InlineData("(1 0 0)", "(1 0 0)", "(1 0 0)")]
		[InlineData("(1 0 0)", "(1 2 3)", "(1 2 3)")]
		[InlineData("(-1 0 0)", "(1 0 0)", "(-1 0 0)")]
		[InlineData("(-1 0 0)", "(1 2 3)", "(-1 -2 -3)")]
		// Calculated rotations
		[InlineData("(0 1 0)", "(1 0 0)", "(0 -1 0)")]
		[InlineData("(0 0 1)", "(1 0 0)", "(0 0 -1)")]
		[InlineData("(0 0 -1)", "(1 0 0)", "(0 0 1)")]
		public void Cartesian3dCoordinate_TransposeFromReferential_Expected(string originCoordinate, string vectorCoordinate, string expectedCoordiante)
		{
			// 1. Prepare
			Cartesian3dCoordinate origin = new Cartesian3dCoordinate(originCoordinate);
			Cartesian3dCoordinate vector = new Cartesian3dCoordinate(vectorCoordinate);
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCoordiante);

			// 2. Execute
			Cartesian3dCoordinate r = vector.TransposeFromReferential(origin);

			// 3. Verify
			Assert.Equal(expected.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}