using System;
using Twisty.Engine.Geometry;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class SphericalVectorTest
	{
		#region Test Methods

		[Theory]
		[InlineData(0.0, 0.0)]
		[InlineData(0.1, 0.1)]
		[InlineData(1.0, 2.0)]
		[InlineData(Math.PI* 1.5, Math.PI - 0.0000001)]
		[InlineData(Math.PI* 1.9, Math.PI- 0.0000000001)]
		public void SphericalVector_CreateWithCorrectInt_ShouldBeInchanged(double phi, double theta)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			SphericalVector o = new SphericalVector(phi, theta);

			// 3. Verify
			Assert.Equal(phi, o.Phi);
			Assert.Equal(theta, o.Theta);
		}

		[Theory]
		[InlineData(-2.0, -1.0, Math.PI -2.0, 1.0)]
		[InlineData(Math.PI* 2.0, Math.PI* 2.0, 0.0, 0.0)]
		[InlineData(Math.PI* 2.0 + 0.1, Math.PI* 2.0 + 0.1, 0.1, 0.1)]
		[InlineData(Math.PI* 2.0 + 0.6, Math.PI* 2.0 + 0.5, 0.6, 0.5)]
		[InlineData(1.0, Math.PI, 0.0, Math.PI)]
		[InlineData(1.0, 0.0, 0.0, 0.0)]
		public void SphericalVector_CreateWithIntToNormalize_ShouldBeExpected(double phi, double theta, double expectedPhi, double expectedTheta)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			SphericalVector o = new SphericalVector(phi, theta);

			// 3. Verify
			Assert.Equal(expectedPhi, o.Phi, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expectedTheta, o.Theta, GeometryAssert.PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}