using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CircularVectorComparerTest
	{
		#region Test Data

		// (CartesianCoordinate x, CartesianCoordinate y, CartesianCoordinate startVector, int expected)
		public static readonly TheoryData<CartesianCoordinate, CartesianCoordinate, CartesianCoordinate, int> ComparedCartesianValues
			= new TheoryData<CartesianCoordinate, CartesianCoordinate, CartesianCoordinate, int>()
		{
			// Perfect equality
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), 0 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 1.0, 0.0), 0 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(0.0, 0.0, 1.0), 0 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 1.0, 1.0), 0 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(7.0, 5.0, 6.0), 0 },
			// Different vector with same angle relative to axis.
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, 1.0), new CartesianCoordinate(0.0, 1.0, 0.0), 0 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 0.0, -1.0), new CartesianCoordinate(0.0, 1.0, 0.0), 0 },
			{ new CartesianCoordinate(0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(0.0, 0.0, -1.0), 0 },
			{ new CartesianCoordinate(0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(0.0, -1.0, 0.0), 0 },
			{ new CartesianCoordinate(0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(0.0, 1.0, 0.0), 0 },
			// Differences.
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 1.0, 0.0), new CartesianCoordinate(0.0, 1.0, 0.0), 1 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, -1.0, 0.0), new CartesianCoordinate(0.0, 1.0, 0.0), -1 },
			{ new CartesianCoordinate(1.0, 0.0, 7.0), new CartesianCoordinate(1.0, 0.0, 3.0), new CartesianCoordinate(1.0, 1.0, 1.0), 1 },
			{ new CartesianCoordinate(0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-1.0, 0.0, 0.0), 1 },
			{ new CartesianCoordinate(0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(-0.5, 0.5, 0.70710678118654757), new CartesianCoordinate(1.0, 0.0, 0.0), -1 },
			// Triange of differents values should not loop.
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 1.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), -1 },
			{ new CartesianCoordinate(1.0, 0.0, 0.0), new CartesianCoordinate(1.0, 2.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), -1 },
			{ new CartesianCoordinate(1.0, 1.0, 0.0), new CartesianCoordinate(1.0, 2.0, 0.0), new CartesianCoordinate(1.0, 0.0, 0.0), -1 },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CircularVectorComparerTest.ComparedCartesianValues), MemberType = typeof(CircularVectorComparerTest))]
		public void CircularVectorComparer_CompareVector_BeExpected(CartesianCoordinate x, CartesianCoordinate y, CartesianCoordinate startVector, int expected)
		{
			// 1. Prepare
			var comparer = new CircularVectorComparer(startVector);

			// 2. Execute
			int result = comparer.Compare(x, y);
			int reverse = comparer.Compare(y, x);

			// 3. Verify
			Assert.Equal(expected, result);
			Assert.Equal(-expected, reverse);
		}

		#endregion Test Methods
	}
}
