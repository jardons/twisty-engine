using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	public class ParametricLineTest
	{

		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		//(CartesianCoordinate p1, CartesianCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double, double, double> ParametricLinesFromPoints = new TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double, double, double>()
		{
			{new CartesianCoordinate(0.0, 0.0, 0.0), new CartesianCoordinate(4.0, 5.0, 6.0), 0.0,4.0,0.0,5.0,0.0,6.0},
			{new CartesianCoordinate(1.0, 1.0, 1.0), new CartesianCoordinate(4.0, 5.0, 6.0), 1.0,3.0,1.0,4.0,1.0,5.0},
			{new CartesianCoordinate(1.0, 2.0, 3.0), new CartesianCoordinate(4.0, 5.0, 6.0), 1.0,3.0,2.0,3.0,3.0,3.0},
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(ParametricLineTest.ParametricLinesFromPoints), MemberType = typeof(ParametricLineTest))]
		public void ParametricLine_CreateFromTwoPoints_BeExpected(CartesianCoordinate p1, CartesianCoordinate p2, double x, double xt, double y, double yt, double z, double zt)
		{
			// 1. Prepare
			// Nothing to prepare.

			// 2. Execute
			ParametricLine o = ParametricLine.FromTwoPoints(p1, p2);

			// 3. Verify
			Assert.Equal(x, o.X, PRECISION_DOUBLE);
			Assert.Equal(xt, o.A, PRECISION_DOUBLE);
			Assert.Equal(y, o.Y, PRECISION_DOUBLE);
			Assert.Equal(yt, o.B, PRECISION_DOUBLE);
			Assert.Equal(z, o.Z, PRECISION_DOUBLE);
			Assert.Equal(zt, o.C, PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}
