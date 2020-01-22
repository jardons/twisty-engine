using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class PlanePositionPointComparerTest
	{
		#region Test Methods

		/*[Theory]
		// Perfect equality
		[InlineData("(1.0 0.0 0.0 0.0)", "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", 0)]
		[InlineData("(0.0 1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", 0)]
		[InlineData("(0.0 0.0 1.0 0.0)", "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", 0)]
		// Differences on one coordinate with a plan perpendicular to cartesian axis.
		[InlineData("(0.0 0.0 1.0 2.0)", "(1.0 0.0 0.0)", "(2.0 0.0 0.0)", -1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(1.0 0.0 0.0)", "(-1.0 0.0 0.0)", 1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(1.0 0.0 0.0)", "(1.0 1.0 0.0)", -1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(0.5 -0.5 0.0)", "(-0.5 -0.5 0.0)", 1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(0.5 -0.5 0.0)", "(-0.5 0.5 0.0)", 1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(0.5 -0.5 0.0)", "(0.5 0.5 0.0)", -1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(0.5 0.5 0.0)", "(-0.5 0.5 0.0)", 1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(0.5 0.5 0.0)", "(-0.5 -0.5 0.0)", 1)]
		[InlineData("(0.0 0.0 1.0 2.0)", "(-0.5 -0.5 0.0)", "(-0.5 0.5 0.0)", -1)]
		[InlineData("(0.0 0.0 -1.0 2.0)", "(1.0 0.0 0.0)", "(2.0 0.0 0.0)", 1)]
		[InlineData("(0.0 0.0 -1.0 2.0)", "(1.0 0.0 0.0)", "(-1.0 0.0 0.0)", -1)]
		[InlineData("(0.0 0.0 -1.0 2.0)", "(1.0 0.0 0.0)", "(1.0 1.0 0.0)", -1)]
		// Differences on two coordinate with a plan perpendicular to cartesian axis.
		[InlineData("(0.0 0.0 1.0 2.0)", "(1.0 2.0 0.0)", "(2.0 1.0 0.0)", -1)]
		[InlineData("(0.0 0.0 -1.0 2.0)", "(1.0 2.0 0.0)", "(2.0 1.0 0.0)", 1)]*/
		public void PlanePositionPointComparerTest_ComparePoints_BeExpected(string planeCoordinates, string firstCoordinates, string secondCoordinates, int expected)
		{
			// 1. Prepare
			Plane p = new Plane(planeCoordinates);
			Cartesian3dCoordinate x = new Cartesian3dCoordinate(firstCoordinates);
			Cartesian3dCoordinate y = new Cartesian3dCoordinate(secondCoordinates);
			var comparer = new PlanePositionPointComparer(p);

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
