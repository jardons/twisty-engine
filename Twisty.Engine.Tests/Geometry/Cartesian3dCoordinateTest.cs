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

		#endregion Test Data

		#region Test Methods

		[Theory]
		[InlineData("(0.0 0.0 0.0)", 0.0, 0.0, 0.0)]
		[InlineData("(1.2 0.0 0.0)", 1.2, 0.0, 0.0)]
		[InlineData("(0.0 1.3 0.0)", 0.0, 1.3, 0.0)]
		[InlineData("(0.0 0.0 1.4)", 0.0, 0.0, 1.4)]
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
		[InlineData("(0.0 0.0 0.0)", "(0.0 0.0 0.0)", "(0.0 0.0 0.0)")]
		[InlineData("(0.0 0.0 0.0)", "(10.0 0.0 0.0)", "(10.0 0.0 0.0)")]
		[InlineData("(1.0 0.0 0.0)", "(-1.0 0.0 0.0)", "(0.0 0.0 0.0)")]
		[InlineData("(0.0 1.0 0.0)", "(0.0 -1.0 0.0)", "(0.0 0.0 0.0)")]
		[InlineData("(0.0 0.0 1.0)", "(0.0 0.0 -1.0)", "(0.0 0.0 0.0)")]
		[InlineData("(0.0 0.0 1.0)", "(1.0 0.0 0.0)", "(1.0 0.0 1.0)")]
		[InlineData("(1.0 0.0 1.0)", "(-1.0 0.0 0.0)", "(0.0 0.0 1.0)")]
		public void Cartesian3dCoordinate_AddVector_BeExpected(string sourceCc, string addedCc, string expectedCc)
		{
			// 1. Prepare
			Cartesian3dCoordinate source = new Cartesian3dCoordinate(sourceCc);
			Cartesian3dCoordinate added = new Cartesian3dCoordinate(addedCc);
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCc);

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
		[InlineData("(0.0 0.0 0.0)", "(0.0 0.0 0.0)", "(0.0 0.0 0.0)")]
		[InlineData("(0.0 0.0 0.0)", "(10.0 0.0 0.0)", "(-10.0 0.0 0.0)")]
		[InlineData("(1.0 0.0 0.0)", "(-1.0 0.0 0.0)", "(2.0 0.0 0.0)")]
		[InlineData("(0.0 1.0 0.0)", "(0.0 -1.0 0.0)", "(0.0 2.0 0.0)")]
		[InlineData("(0.0 0.0 1.0)", "(0.0 0.0 -1.0)", "(0.0 0.0 2.0)")]
		[InlineData("(0.0 0.0 1.0)", "(1.0 0.0 0.0)", "(-1.0 0.0 1.0)")]
		[InlineData("(1.0 0.0 1.0)", "(-1.0 0.0 0.0)", "(2.0 0.0 1.0)")]
		public void Cartesian3dCoordinate_SubstractVector_BeExpected(string sourceCc, string substractedCc, string expectedCc)
		{
			// 1. Prepare
			Cartesian3dCoordinate source = new Cartesian3dCoordinate(sourceCc);
			Cartesian3dCoordinate substracted = new Cartesian3dCoordinate(substractedCc);
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCc);

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
		[InlineData("(0.0 1.0 0.0)", "(0.0 1.0 0.0)", 0.0)]
		[InlineData("(0.0 1.0 0.0)", "(-1.0 0.0 0.0)", Math.PI / 2.0)]
		[InlineData("(1 0 0)", "(-1 0 0)", Math.PI)]
		[InlineData("(1 0 0)", "(1 1 0)", Math.PI / 4.0)]
		[InlineData("(0 0 1)", "(1 0 1)", Math.PI / 4.0)]
		public void Cartesian3dCoordinate_CalculateThetaBetweenVector_ReturnExpected(string c1Cc, string c2Cc, double expected)
		{
			// 1. Prepare
			Cartesian3dCoordinate c1 = new Cartesian3dCoordinate(c1Cc);
			Cartesian3dCoordinate c2 = new Cartesian3dCoordinate(c2Cc);

			// 2. Execute
			double r1 = c1.GetThetaTo(c2);
			double r2 = c2.GetThetaTo(c1);

			// 3. Verify
			Assert.Equal(expected, r1, PRECISION_DOUBLE);
			Assert.Equal(expected, r2, PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(4 5 6)", 1.0, "(4 5 6)")]
		[InlineData("(4 8 6)", 0.5, "(2 4 3)")]
		[InlineData("(4 8 6)", 1.5, "(6 12 9)")]
		[InlineData("(4 8 -6)", -2.0, "(-8 -16 12)")]
		public void Cartesian3dCoordinate_MultiplyByDouble_ReturnExpected(string cc, double val, string expectedCc)
		{
			// 1. Prepare
			Cartesian3dCoordinate original = new Cartesian3dCoordinate(cc);
			Cartesian3dCoordinate expected = new Cartesian3dCoordinate(expectedCc);

			// 2. Execute
			Cartesian3dCoordinate r1 = original * val;
			Cartesian3dCoordinate r2 = val * original;

			// 3. Verify
			Assert.Equal(expected.X, r1.X, PRECISION_DOUBLE);
			Assert.Equal(expected.X, r2.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r1.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, r2.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r1.Z, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, r2.Z, PRECISION_DOUBLE);
		}

		[Theory]
		// Exceptions to formula
		[InlineData("(1 0 0)", "(1 0 0)", "(1 0 0)")]
		[InlineData("(1 0 0)", "(1 2 3)", "(1 2 3)")]
		[InlineData("(-1 0 0)", "(1 0 0)", "(-1 0 0)")]
		[InlineData("(-1 0 0)", "(1 2 3)", "(-1 -2 -3)")]
		// Calculated rotations
		[InlineData("(0 1 0)", "(1 0 0)", "(0 1 0)")]
		[InlineData("(0 0 1)", "(1 0 0)", "(0 0 1)")]
		[InlineData("(0 0 -1)", "(1 0 0)", "(0 0 -1)")]
		[InlineData("(1 1 1)", "(1 0 0)", "(1 1 1)")]
		[InlineData("(0 1 0)", "(1 2 3)", "(-2 1 3)")]
		[InlineData("(1 1 0)", "(0 1 0)", "(-1 1 0)")]
		[InlineData("(1 1 0)", "(-1 0 0)", "(-1 -1 0)")]
		[InlineData("(1 1 1)", "(1 1 0)", "(0 2 1)")]
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
			Assert.Equal(expected.Z, r.Z, PRECISION_DOUBL
		}

		[Theory]
		[InlineData("(1 1 1)", "(0.577350269189626 0.577350269189626 0.577350269189626)")]
		[InlineData("(0.5 0.5 0.5)", "(0.577350269189626 0.577350269189626 0.577350269189626)")]
		[InlineData("(2 2 2)", "(0.577350269189626 0.577350269189626 0.577350269189626)")]
		[InlineData("(666 666 666)", "(0.577350269189626 0.577350269189626 0.577350269189626)")]
		[InlineData("(1 0 0)", "(1 0 0)")]
		[InlineData("(0 1 0)", "(0 1 0)")]
		[InlineData("(0 0 1)", "(0 0 1)")]
		[InlineData("(10 0 0)", "(1 0 0)")]
		[InlineData("(0 10 0)", "(0 1 0)")]
		[InlineData("(0 0 10)", "(0 0 1)")]
		[InlineData("(4 5 6)", "(0.455842305838552 0.56980288229819 0.683763458757828)")]
		[InlineData("(3 2 1)", "(0.801783725737273 0.534522483824849 0.267261241912424)")]
		public void Cartesian3dCoordinate_NormalizeValues_ShouldMatch(string coord, string expected)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(coord);
			Cartesian3dCoordinate expectedCoordinates = new Cartesian3dCoordinate(expected);

			// 2. Execute
			Cartesian3dCoordinate r = cc.Normalize();

			// 3. Verify
			Assert.Equal(expectedCoordinates.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(expectedCoordinates.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(expectedCoordinates.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData("(0 0 0)", "(0 0 0)")]
		[InlineData("(1 1 1)", "(1 1 1)")]
		[InlineData("(-1 2 3)", "(-1 2 3)")]
		[InlineData("(0.5 0.5 0.5)", "(0.5 0.5 0.5)")]
		[InlineData("(0.5 0.5 0.5)", "(0.4999999999999 0.5000000000001 0.5)")]
		public void Cartesian3dCoordinate_IsSamePoint_True(string cc1, string cc2)
		{
			// 1. Prepare
			Cartesian3dCoordinate c1 = new Cartesian3dCoordinate(cc1);
			Cartesian3dCoordinate c2 = new Cartesian3dCoordinate(cc2);

			// 2. Execute
			bool r1 = c1.IsSamePoint(c2);
			bool r2 = c2.IsSamePoint(c1);

			// 3. Verify
			Assert.True(r1);
			Assert.True(r2);
		}

		[Theory]
		[InlineData("(0 0 0)", "(1 1 1)")]
		[InlineData("(1 1 1)", "(1 2 1)")]
		[InlineData("(-1 2 3)", "(1 2 3)")]
		[InlineData("(0.5 0.5 0.5)", "(1 1 1)")]
		[InlineData("(0.5 0.5 0.5)", "(0.4999999 0.5 0.5)")]
		[InlineData("(0.5 0.5 0.5)", "(0.5 0.500001 0.5)")]
		public void Cartesian3dCoordinate_IsSamePoint_False(string cc1, string cc2)
		{
			// 1. Prepare
			Cartesian3dCoordinate c1 = new Cartesian3dCoordinate(cc1);
			Cartesian3dCoordinate c2 = new Cartesian3dCoordinate(cc2);

			// 2. Execute
			bool r1 = c1.IsSamePoint(c2);
			bool r2 = c2.IsSamePoint(c1);

			// 3. Verify
			Assert.False(r1);
			Assert.False(r2);
		}

		[Theory]
		[InlineData("(0 0 0)", "(0 0 0)")]
		[InlineData("(1 1 1)", "(1 1 1)")]
		[InlineData("(1 1 1)", "(2 2 2)")]
		[InlineData("(-1 2 3)", "(-1 2 3)")]
		[InlineData("(0.5 0.5 0.5)", "(0.5 0.5 0.5)")]
		[InlineData("(0.5 0.5 0.5)", "(1 1 1)")]
		[InlineData("(0.5 0.5 0.5)", "(0.4999999999999 0.5000000000001 0.5)")]
		public void Cartesian3dCoordinate_IsSameVector_True(string cc1, string cc2)
		{
			// 1. Prepare
			Cartesian3dCoordinate c1 = new Cartesian3dCoordinate(cc1);
			Cartesian3dCoordinate c2 = new Cartesian3dCoordinate(cc2);

			// 2. Execute
			bool r1 = c1.IsSameVector(c2);
			bool r2 = c2.IsSameVector(c1);

			// 3. Verify
			Assert.True(r1);
			Assert.True(r2);
		}

		[Theory]
		[InlineData("(0 0 0)", "(1 1 1)")]
		[InlineData("(1 1 1)", "(1 2 1)")]
		[InlineData("(-1 2 3)", "(1 2 3)")]
		[InlineData("(0.5 0.5 0.5)", "(1 2 3)")]
		[InlineData("(0.5 0.5 0.5)", "(0.4999999 0.5 0.5)")]
		[InlineData("(0.5 0.5 0.5)", "(0.5 0.500001 0.5)")]
		public void Cartesian3dCoordinate_IsSameVector_False(string cc1, string cc2)
		{
			// 1. Prepare
			Cartesian3dCoordinate c1 = new Cartesian3dCoordinate(cc1);
			Cartesian3dCoordinate c2 = new Cartesian3dCoordinate(cc2);

			// 2. Execute
			bool r1 = c1.IsSameVector(c2);
			bool r2 = c2.IsSameVector(c1);

			// 3. Verify
			Assert.False(r1);
			Assert.False(r2);
		}

		#endregion Test Methods
	}
}