using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CartesianCoordinatesConverterTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Data

		//(string cPlane, string c3d, string c2d)
		public static readonly TheoryData<string, string, string> Cartesian3dCoordinates = new TheoryData<string, string, string>()
		{
			{ "(1 0 0 5)", "(0 0 0)", "(0 0)" },
			{ "(-1 0 0 5)", "(0 0 0)", "(0 0)" },
			{ "(0 1 0 5)", "(0 0 0)", "(0 0)" },
			{ "(0 0 1 5)", "(0 0 9)", "(0 0)" },
			{ "(0 0 1 5)", "(1 0 9)", "(0 -1)" },
			{ "(0 0 1 5)", "(0 1 9)", "(1 0)" },
			{ "(0 0 -1 5)", "(1 0 9)", "(0 1)" },
			{ "(0 0 -1 5)", "(0 1 9)", "(1 0)" },
			{ "(0 1 0 5)", "(1 0 9)", "(-1 9)" },
			{ "(0 1 0 5)", "(0 1 9)", "(0 9)" },
			{ "(0 -1 0 5)", "(1 0 9)", "(1 9)" },
			{ "(0 -1 0 5)", "(0 1 9)", "(0 9)" },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(CartesianCoordinatesConverterTest.Cartesian3dCoordinates), MemberType = typeof(CartesianCoordinatesConverterTest))]
		public void Cartesian3dCoordinatesConverter_Convert3dTo2d_BeExpected(string cPlane, string c3d, string c2d)
		{
			// 1. Prepare
			Plane p = new Plane(cPlane);
			CartesianCoordinatesConverter converter = new CartesianCoordinatesConverter(p);
			Cartesian2dCoordinate expected = new Cartesian2dCoordinate(c2d);
			Cartesian3dCoordinate source = new Cartesian3dCoordinate(c3d);

			// 2. Execute
			Cartesian2dCoordinate result = converter.ConvertTo2d(source);

			// 3. Verify
			Assert.Equal(expected.X, result.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, result.Y, PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}
