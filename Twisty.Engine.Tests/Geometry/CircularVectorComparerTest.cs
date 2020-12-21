using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CircularVectorComparerTest
	{
		#region Test Methods

		[Theory]
		// Perfect equality
		[InlineData("(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(1.0 0.0 0.0 1)", 0)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(0.0 1.0 0.0 1)", 0)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(0.0 0.0 1.0 1)", 0)]
		// Differences.
		[InlineData("(1.0 0.0 0.0)", "(1.0 -1.0 0.0)", "(0 0 1 1)", 1)]
		[InlineData("(1.0 0.0 7.0)", "(1.0 0.0 3.0)", "(0 1 0 1)", -1)]
		[InlineData("(0.5 0.5 0.70710678118654757)", "(-0.5 0.5 0.70710678118654757)", "(0 -1 0 1)", -1)]
		[InlineData("(0.5 0.5 0.70710678118654757)", "(-0.5 0.5 0.70710678118654757)", "(0 1 0 1)", 1)]
		// Line points of differents values should not loop.
		[InlineData("(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(0 0 1 1)", -1)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1)]
		[InlineData("(1.0 1.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1)]
		// Square of differents values should not loop on standard axis.
		[InlineData("(-0.5 0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 0.5 0.71)", "(-0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		public void CircularVectorComparer_Compare3dVectorWithSimplePlane_BeExpected(string xCoord, string yCoord, string plane, int expected)
			=> CircularVectorComparer_Compare3dVector_BeExpected(xCoord, yCoord, plane, expected);

		[Theory]
		// Square of differents values should not loop on slightly changed axis.
		[InlineData("(-0.5 0.5 0.71)", "(0.5 0.5 0.71)", "(0.01 0 1 -1)", 1)]
		[InlineData("(0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0.01 0 1 -1)", 1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0.01 0 1 -1)", 1)]
		[InlineData("(-0.5 0.5 0.71)", "(0.5 -0.5 0.71)", "(0.01 0 1 -1)", 1)]
		[InlineData("(-0.5 0.5 0.71)", "(-0.5 -0.5 0.71)", "(0.01 0 1 -1)", 1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 -0.5 0.71)", "(0.01 0 1 -1)", 1)]
		// Square of differents values should not loop on diagonal axis going throug it.
		// -2 2 < 2 2 < 2 -2 < -2 -2
		[InlineData("(-2 2 0.71)", "(2 2 0.71)", "(1 1 1 -10)", -1)]
		[InlineData("(2 2 0.71)", "(2 -2 0.71)", "(1 1 1 -10)", -1)]
		[InlineData("(2 2 0.71)", "(-2 -2 0.71)", "(1 1 1 -10)", -1)]
		[InlineData("(-2 2 0.71)", "(2 -2 0.71)", "(1 1 1 -10)", -1)]
		[InlineData("(-2 2 0.71)", "(-2 -2 0.71)", "(1 1 1 -10)", -1)]
		[InlineData("(2 -2 0.71)", "(-2 -2 0.71)", "(1 1 1 -10)", -1)]
		public void CircularVectorComparer_Compare3dVectorWithComplexPlane_BeExpected(string xCoord, string yCoord, string plane, int expected)
			=> CircularVectorComparer_Compare3dVector_BeExpected(xCoord, yCoord, plane, expected);

		[Theory]
		// Line Points should not loop when staying on same side of the center.
		[InlineData("(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(0.5 0.5 0.5)", "(0 0 1 -1)", -1)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 2.0 0.0)", "(0.5 0.5 0.5)", "(0 0 1 -1)", -1)]
		[InlineData("(1.0 1.0 0.0)", "(1.0 2.0 0.0)", "(0.5 0.5 0.5)", "(0 0 1 -1)", -1)]
		// Line Points should not loop when switching side of the center.
		[InlineData("(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(2 2 2)", "(0 0 1 -1)", 1)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 2.0 0.0)", "(2 2 2)", "(0 0 1 -1)", 1)]
		[InlineData("(1.0 1.0 0.0)", "(1.0 2.0 0.0)", "(2 2 2)", "(0 0 1 -1)", 1)]
		// Square of differents values should not loop on slightly changed axis.
		// 1.5 1.5 > 1.5 0.5 > 0.5 0.5 > 0.5 1.5
		[InlineData("(0.5 1.5 0.71)", "(1.5 1.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		[InlineData("(1.5 0.5 0.71)", "(1.5 1.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		[InlineData("(0.5 0.5 0.71)", "(1.5 1.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		[InlineData("(0.5 1.5 0.71)", "(1.5 0.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		[InlineData("(0.5 1.5 0.71)", "(0.5 0.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		[InlineData("(0.5 0.5 0.71)", "(1.5 0.5 0.71)", "(1 1 1)", "(0 0 1 -1)", -1)]
		public void CircularVectorComparer_Compare3dVectorWithCenter_BeExpected(string xCoord, string yCoord, string centerCoord, string plane, int expected)
		{
			// 1. Prepare
			Cartesian3dCoordinate x = new Cartesian3dCoordinate(xCoord);
			Cartesian3dCoordinate y = new Cartesian3dCoordinate(yCoord);
			Cartesian3dCoordinate c = new Cartesian3dCoordinate(centerCoord);
			Plane p = new Plane(plane);
			var comparer = new CircularVectorComparer(p, c);

			// 2. Execute
			int result = comparer.Compare(x, y);
			int reverse = comparer.Compare(y, x);

			// 3. Verify
			Assert.Equal(expected, result);
			Assert.Equal(-expected, reverse);
		}

		[Theory]
		// Perfect equality
		[InlineData("(1 0)", "(1 0)", 0)]
		[InlineData("(1 1)", "(1 1)", 0)]
		[InlineData("(0 1)", "(0 1)", 0)]
		// Differences step by step.
		[InlineData("(1 0)", "(1 1)", -1)]
		[InlineData("(1 1)", "(0 1)", -1)]
		[InlineData("(0 1)", "(-1 1)", -1)]
		[InlineData("(-1 1)", "(-1 0)", -1)]
		[InlineData("(-1 0)", "(-1 -1)", -1)]
		[InlineData("(-1 -1)", "(0 -1)", -1)]
		[InlineData("(0 -1)", "(1 -1)", -1)]
		// Differences Opposites
		[InlineData("(1 1)", "(-1 -1)", -1)]
		[InlineData("(1 0)", "(-1 0)", -1)]
		public void CircularVectorComparer_Compare2dVector_BeExpected(string xCoord, string yCoord, int expected)
		{
			// 1. Prepare
			Cartesian2dCoordinate x = new Cartesian2dCoordinate(xCoord);
			Cartesian2dCoordinate y = new Cartesian2dCoordinate(yCoord);

			// Plane has no impact in 2d, no need to change it.
			Plane p = new Plane("(1.0 0.0 0.0 1)");
			var comparer = new CircularVectorComparer(p);

			// 2. Execute
			int result = comparer.Compare(x, y);
			int reverse = comparer.Compare(y, x);

			// 3. Verify
			Assert.Equal(expected, result);
			Assert.Equal(-expected, reverse);
		}

		#endregion Test Methods

		#region Private Methods

		private void CircularVectorComparer_Compare3dVector_BeExpected(string xCoord, string yCoord, string plane, int expected)
		{
			// 1. Prepare
			Cartesian3dCoordinate x = new Cartesian3dCoordinate(xCoord);
			Cartesian3dCoordinate y = new Cartesian3dCoordinate(yCoord);
			Plane p = new Plane(plane);
			var comparer = new CircularVectorComparer(p);

			// 2. Execute
			int result = comparer.Compare(x, y);
			int reverse = comparer.Compare(y, x);

			// 3. Verify
			Assert.Equal(expected, result);
			Assert.Equal(-expected, reverse);
		}

		#endregion Private Methods
	}
}
