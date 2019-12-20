using System;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class SphericalVectorTest
	{
		private const int PRECISION_DOUBLE = 10;

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
			Assert.Equal(expectedPhi, o.Phi, PRECISION_DOUBLE);
			Assert.Equal(expectedTheta, o.Theta, PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData(0.0, 0.0, 0.0, 0.0, true)]
		[InlineData(5.0, 10.0, 5.0, 10.0, true)]
		[InlineData(0.5, 0.10, 0.5, 0.11, false)]
		[InlineData(5, 10, 50, 10, false)]
		[InlineData(Math.PI* 2.0, Math.PI* 2.0, 0.0, 0.0, true)]
		public void SphericalVector_CompareSphericalVector_ShouldBeExpected(double phi1, double theta1, double phi2, double theta2, bool expected)
		{
			// 1. Prepare
			SphericalVector o1 = new SphericalVector(phi1, theta1);
			SphericalVector o2 = new SphericalVector(phi2, theta2);

			// 2. Execute
			bool resultEquals1 = o1.Equals(o2);
			bool resultEquals2 = o2.Equals(o1);
			bool resultEqualsOperator1 = o1 == o2;
			bool resultEqualsOperator2 = o2 == o1;
			bool resultDifferentOperator1 = o1 != o2;
			bool resultDifferentOperator2 = o2 != o1;

			// 3. Verify
			Assert.Equal(expected, resultEquals1);
			Assert.Equal(expected, resultEquals2);
			Assert.Equal(expected, resultEqualsOperator1);
			Assert.Equal(expected, resultEqualsOperator2);
			Assert.Equal(!expected, resultDifferentOperator1);
			Assert.Equal(!expected, resultDifferentOperator2);
		}

		#endregion Test Methods
	}
}