using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CircularVectorComparerTest
	{
		#region Test Data

		// (string xCoord, strinct yCoord, string plane, int expected)
		public static readonly TheoryData<string, string, string, int> ComparedCartesianValues = new TheoryData<string, string, string, int>()
		{
			// Perfect equality
			{ "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(1.0 0.0 0.0 1)", 0 },
			{ "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(0.0 1.0 0.0 1)", 0 },
			{ "(1.0 0.0 0.0)", "(1.0 0.0 0.0)", "(0.0 0.0 1.0 1)", 0 },
			// Differences.
			{ "(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(0 0 1 1)", -1 },
			{ "(1.0 0.0 0.0)", "(1.0 -1.0 0.0)", "(0 0 1 1)", -1 },
			{ "(1.0 0.0 7.0)", "(1.0 0.0 3.0)", "(0 1 0 1)", 1 },
			{ "(0.5 0.5 0.70710678118654757)", "(-0.5 0.5 0.70710678118654757)", "(0 -1 0 1)", 1 },
			{ "(0.5 0.5 0.70710678118654757)", "(-0.5 0.5 0.70710678118654757)", "(0 1 0 1)", -1 },
			// Triangle of differents values should not loop.
			{ "(1.0 0.0 0.0)", "(1.0 1.0 0.0)", "(0 0 1 1)", -1 },
			{ "(1.0 0.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1 },
			{ "(1.0 1.0 0.0)", "(1.0 2.0 0.0)", "(0 0 1 1)", -1 },
			// Square rotation full test
			{ "(0.5 0.5 0.71)", "(-0.5 0.5 0.71)", "(0 0 1 1)", -1 },
			{ "(0.5 0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1 },
			{ "(0.5 0.5 0.71)", "(-0.5 -0.5 0.71)", "(0 0 1 1)", -1 },
			{ "(-0.5 0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1 },
			{ "(-0.5 0.5 0.71)", "(-0.5 -0.5 0.71)", "(0 0 1 1)", -1 },
			{ "(-0.5 -0.5 0.71)", "(0.5 -0.5 0.71)", "(0 0 1 1)", -1 },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CircularVectorComparerTest.ComparedCartesianValues), MemberType = typeof(CircularVectorComparerTest))]
		public void CircularVectorComparer_CompareVector_BeExpected(string xCoord, string yCoord, string plane, int expected)
		{
			// 1. Prepare
			CartesianCoordinate x = new CartesianCoordinate(xCoord);
			CartesianCoordinate y = new CartesianCoordinate(yCoord);
			Plane p = new Plane(plane);
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
