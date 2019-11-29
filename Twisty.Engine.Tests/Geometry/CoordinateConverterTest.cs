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

		//(double phi, double theta, double x, double y, double z)
		public static readonly TheoryData<double, double, double, double, double> SphericalAndCartesian = new TheoryData<double, double, double, double, double>()
		{
			{0.0, 0.0, 0.0, 0.0, 1.0},
			{Math.PI / 2.0, Math.PI / 2.0, 0.0, 1.0, 0.0},
			{Math.PI, Math.PI / 2.0, -1.0, 0.0, 0.0},
			{0.0, Math.PI, 0.0, 0.0, -1.0},
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
		[MemberData(nameof(CoordinateConverterTest.SphericalAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromSphericalToCartesian_BeExpected(double phi, double theta, double x, double y, double z)
		{
			// 1. Prepare
			SphericalVector sc = new SphericalVector(phi, theta);

			// 2. Execute
			CartesianCoordinate cc = CoordinateConverter.ConvertToCartesian(sc);

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
			CartesianCoordinate cc = new CartesianCoordinate(x, y, z);

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
			HomogeneousCoordiante hc = new HomogeneousCoordiante(hx, hy, hz, hw);

			// 2. Execute
			CartesianCoordinate cc = CoordinateConverter.ConvertToCartesian(hc);

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
			CartesianCoordinate cc = new CartesianCoordinate(x, y, z);

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
