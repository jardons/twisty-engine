using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class PlaneTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double> CreationFromNormal = new TheoryData<CartesianCoordinate, CartesianCoordinate, double, double, double, double>()
		{
			{new CartesianCoordinate(2.0, 3.0, 4.0), new CartesianCoordinate(1.0, 2.0, 3.0), 2.0, 3.0, 4.0, -20.0}
		};

		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, ParametricLine, CartesianCoordinate> LineIntersection = new TheoryData<CartesianCoordinate, CartesianCoordinate, ParametricLine, CartesianCoordinate>()
		{
			{new CartesianCoordinate(2.0, 3.0, 4.0), new CartesianCoordinate(1.0, 2.0, 3.0), new ParametricLine(-2.0, 0.0, 0.0, 6.0, 5.0, 6.0), new CartesianCoordinate(0.823529411764706, 2.35294117647059, 2.82352941176471)}
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromNormal), MemberType = typeof(PlaneTest))]
		public void Plane_CreateFromNormalAndPoint_BeExpected(CartesianCoordinate normal, CartesianCoordinate point, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			// Nothing to prepare

			// 2. Execute
			Plane p = new Plane(normal, point);
			bool isOnPlane = p.IsOnPlane(point);

			// 3. Verify
			Assert.Equal(p.A, expectedA, PRECISION_DOUBLE);
			Assert.Equal(p.B, expectedB, PRECISION_DOUBLE);
			Assert.Equal(p.C, expectedC, PRECISION_DOUBLE);
			Assert.Equal(p.D, expectedD, PRECISION_DOUBLE);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.CreationFromNormal), MemberType = typeof(PlaneTest))]
		public void Plane_CreateFromNormalAndPointCheckIsOnPlane_True(CartesianCoordinate normal, CartesianCoordinate point, double expectedA, double expectedB, double expectedC, double expectedD)
		{
			// 1. Prepare
			Plane p = new Plane(normal, point);

			// 2. Execute
			bool isOnPlane = p.IsOnPlane(point);

			// 3. Verify
			Assert.True(isOnPlane);
		}

		[Theory]
		[MemberData(nameof(PlaneTest.LineIntersection), MemberType = typeof(PlaneTest))]
		public void Plane_GetIntersection_Expected(CartesianCoordinate normal, CartesianCoordinate point, ParametricLine line, CartesianCoordinate expected)
		{
			// 1. Prepare
			Plane p = new Plane(normal, point);

			// 2. Execute
			CartesianCoordinate r = p.GetIntersection(line);

			// 3. Verify
			Assert.Equal(r.X, expected.X, PRECISION_DOUBLE);
			Assert.Equal(r.Y, expected.Y, PRECISION_DOUBLE);
			Assert.Equal(r.Z, expected.Z, PRECISION_DOUBLE);
		}

		

		#endregion Test Methods
	}
}
