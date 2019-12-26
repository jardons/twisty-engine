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
		// Triangle of differents values should not loop.
		[InlineData("(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(0 0 1 1)", -1)]
		[InlineData("(1.0 0.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1)]
		[InlineData("(1.0 1.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1)]
		// Square rotation full test
		[InlineData("(-0.5 0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 0.5 0.71)", "(-0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		[InlineData("(-0.5 -0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1)]
		public void CircularVectorComparer_CompareVector_BeExpected(string xCoord, string yCoord, string plane, int expected)
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
	}
}
