using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CoordinateConverterTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		public static readonly TheoryData<double, double, double, double, double> SphericalAndCartesian = new TheoryData<double, double, double, double, double>()
		{
			{0.0, 0.0, 0.0, 0.0, 1.0},
			{Math.PI / 2.0, Math.PI / 2.0, 0.0, 1.0, 0.0},
			{Math.PI, Math.PI / 2.0, -1.0, 0.0, 0.0},
			{0.0, Math.PI, 0.0, 0.0, -1.0},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CoordinateConverterTest.SphericalAndCartesian), MemberType = typeof(CoordinateConverterTest))]
		public void CoordinateConverter_ConvertFromSphericalToCartesian_ShouldBeExpected(double phi, double theta, double x, double y, double z)
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
		public void CoordinateConverter_ConvertFromCartesianToSpherical_ShouldBeExpected(double phi, double theta, double x, double y, double z)
		{
			// 1. Prepare
			CartesianCoordinate cc = new CartesianCoordinate(x, y, z);
			
			// 2. Execute
			var sc = CoordinateConverter.ConvertToSpherical(cc);

			// 3. Verify
			Assert.Equal(phi, sc.Phi);
			Assert.Equal(theta, sc.Theta);
		}

		#endregion Test Methods
	}
}
