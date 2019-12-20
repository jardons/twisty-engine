using System;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CoordinateConverterTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		//(string s, double d1, double d2, double d3, double d4)
		public static readonly TheoryData<string, double, double, double, double> ParseCoordinates = new TheoryData<string, double, double, double, double>()
		{
			{ "(1.0)", 1.0, double.NaN, double.NaN, double.NaN },
			{ "(1.0 2.0)", 1.0, 2.0, double.NaN, double.NaN },
			{ "(1.0 2.0)", 1.0, 2.0, double.NaN, double.NaN },
			{ "( 1.0 2.0 3.123456789)", 1.0, 2.0, 3.123456789, double.NaN },
			{ "(1.0 2.0 3.123456789 -10 )", 1.0, 2.0, 3.123456789, -10.0 },
		};

		//(string s)
		public static readonly TheoryData<string> InvalidCoordinates = new TheoryData<string>()
		{
			{" "},
			{"Hello World !"},
			{"1 2 3"},
			{"(x y z)"},
			{"[1 2 2]"},
			{"(1..2)"},
		};

		//(double phi, double theta, double x, double y, double z)
		public static readonly TheoryData<double, double, double, double, double> SphericalAndCartesian = new TheoryData<double, double, double, double, double>()
		{
			// X axis
			{0.0, Math.PI / 2.0, 1.0, 0.0, 0.0},
			// Y axis
			{Math.PI / 2.0, Math.PI / 2.0, 0.0, 1.0, 0.0},
			// Z axis
			{0.0, 0.0, 0.0, 0.0, 1.0},
			// -X axis
			{Math.PI, Math.PI / 2.0, -1.0, 0.0, 0.0},
			// -Y axis
			{Math.PI / 2.0 * 3.0, Math.PI / 2.0, 0.0, -1.0, 0.0},
			// -Z Axis
			{0.0, Math.PI, 0.0, 0.0, -1.0},
			// Corners
			{Math.PI / 4.0, Math.PI / 4.0, 0.5, 0.5, 0.70710678118654757 },
			{Math.PI / 4.0 * 3.0, Math.PI / 4.0, -0.5, 0.5, 0.70710678118654757 },
		};

		//(double hx, double hy, double hz, double hw, double x, double y, double z)
		public static readonly TheoryData<double, double, double, double, double, double, double> HomogeneousAndCartesian = new TheoryData<double, double, double, double, double, double, double>()
		{
			{1.0, 0.0, 0.0, 1.0, 1.0, 0.0, 0.0},
			{1.0, 2.0, 3.0, 1.0, 1.0, 2.0, 3.0},
			{0.0, 0.0, 9.0, 1.0, 0.0, 0.0, 9.0},
		};

		//(double hx, double hy, double hz, double hw, double x, double y, double z)
		public static readonly TheoryData<double, double, double, double, double, double, double> HomogeneousToCartesianOnly = new TheoryData<double, double, double, double, double, double, double>()
		{
			{2.0, 0.0, 0.0, 2.0, 1.0, 0.0, 0.0},
			{4.0, 8.0, 12.0, 4.0, 1.0, 2.0, 3.0},
			{0.0, 0.0, 4.5, 0.5, 0.0, 0.0, 9.0},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.ParseCoordinates), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ParseCoordinates_BeExpected(string s, double d1, double d2, double d3, double d4)
		{
			// 1. Prepare
			int count = 0;
			if (!double.IsNaN(d1))
				++count;
			if (!double.IsNaN(d2))
				++count;
			if (!double.IsNaN(d3))
				++count;
			if (!double.IsNaN(d4))
				++count;

			// 2. Execute
			double[] vals = CoordinateConverter.ParseCoordinates(s);

			// 3. Verify
			Assert.NotNull(vals);
			Assert.Equal(count, vals.Length);
			if (!double.IsNaN(d1))
				Assert.Equal(d1, vals[0], PRECISION_DOUBLE);
			if (!double.IsNaN(d2))
				Assert.Equal(d2, vals[1], PRECISION_DOUBLE);
			if (!double.IsNaN(d3))
				Assert.Equal(d3, vals[2], PRECISION_DOUBLE);
			if (!double.IsNaN(d4))
				Assert.Equal(d4, vals[3], PRECISION_DOUBLE);
		}

		[Fact]
		public void CoordinateConverter_ParseNullCoordinates_ThrowArgumentNullException()
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Action a = () => CoordinateConverter.ParseCoordinates(null);

			// 3. Verify
			Assert.Throws<ArgumentNullException>(a);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.InvalidCoordinates), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ParseInvalidCoordinates_ThrowFormatException(string s)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Action a = () => CoordinateConverter.ParseCoordinates(s);

			// 3. Verify
			Assert.Throws<FormatException>(a);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.SphericalAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromSphericalToCartesian_BeExpected(double phi, double theta, double x, double y, double z)
		{
			// 1. Prepare
			SphericalVector sc = new SphericalVector(phi, theta);

			// 2. Execute
			Cartesian3dCoordinate cc = CoordinateConverter.ConvertToCartesian(sc);

			// 3. Verify
			Assert.Equal(x, cc.X, PRECISION_DOUBLE);
			Assert.Equal(y, cc.Y, PRECISION_DOUBLE);
			Assert.Equal(z, cc.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.SphericalAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromCartesianToSpherical_BeExpected(double phi, double theta, double x, double y, double z)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(x, y, z);

			// 2. Execute
			var sc = CoordinateConverter.ConvertToSpherical(cc);

			// 3. Verify
			Assert.Equal(phi, sc.Phi);
			Assert.Equal(theta, sc.Theta);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.HomogeneousAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		[MemberData(nameof(CoordinateConverterTest.HomogeneousToCartesianOnly), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromHomogeneousToCartesian_BeExpected(double hx, double hy, double hz, double hw, double x, double y, double z)
		{
			// 1. Prepare
			HomogeneousCoordinate hc = new HomogeneousCoordinate(hx, hy, hz, hw);

			// 2. Execute
			Cartesian3dCoordinate cc = CoordinateConverter.ConvertToCartesian(hc);

			// 3. Verify
			Assert.Equal(x, cc.X, PRECISION_DOUBLE);
			Assert.Equal(y, cc.Y, PRECISION_DOUBLE);
			Assert.Equal(z, cc.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.HomogeneousAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromCartesianToHomogeneous_BeExpected(double hx, double hy, double hz, double hw, double x, double y, double z)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new Cartesian3dCoordinate(x, y, z);

			// 2. Execute
			var hc = CoordinateConverter.ConvertToHomogeneous(cc);

			// 3. Verify
			Assert.Equal(hx, hc.X);
			Assert.Equal(hy, hc.Y);
			Assert.Equal(hz, hc.Z);
			Assert.Equal(hw, hc.W);
		}

		#endregion Test Methods
	}
}
