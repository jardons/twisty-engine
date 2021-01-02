using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CartesianCoordinatesConverterTest
	{
		#region Test Methods

		[Theory]
		// Fixed conversions
		[InlineData("(1 0 0 5)", "(0 0 0)", "(0 0)")]
		[InlineData("(-1 0 0 5)", "(0 0 0)", "(0 0)")]
		[InlineData("(0 1 0 5)", "(0 0 0)", "(0 0)")]
		[InlineData("(0 0 1 5)", "(0 0 9)", "(0 0)")]
		[InlineData("(0 0 1 5)", "(1 0 9)", "(0 -1)")]
		[InlineData("(0 0 1 5)", "(0 1 9)", "(1 0)")]
		[InlineData("(0 0 -1 5)", "(1 0 9)", "(0 1)")]
		[InlineData("(0 0 -1 5)", "(0 1 9)", "(1 0)")]
		[InlineData("(0 1 0 5)", "(1 0 9)", "(-1 9)")]
		[InlineData("(0 1 0 5)", "(0 1 9)", "(0 9)")]
		[InlineData("(0 -1 0 5)", "(1 0 9)", "(1 9)")]
		[InlineData("(0 -1 0 5)", "(0 1 9)", "(0 9)")]
		// Rotated Conversions
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
			Assert.Equal(expected.X, result.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(expected.Y, result.Y, GeometryAssert.PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}
