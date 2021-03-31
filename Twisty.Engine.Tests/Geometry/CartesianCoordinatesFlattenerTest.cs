using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry
{
	[Trait("Category", "Geometry")]
	public class CartesianCoordinatesFlattenerTest
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
		public void CartesianCoordinatesFlattener_Convert3dTo2d_BeExpected(string cPlane, string c3d, string c2d)
		{
			// 1. Prepare
			Plane p = new Plane(cPlane);
			CartesianCoordinatesFlattener converter = new CartesianCoordinatesFlattener(p);
			Cartesian2dCoordinate expected = new Cartesian2dCoordinate(c2d);
			Cartesian3dCoordinate source = new Cartesian3dCoordinate(c3d);

			// 2. Execute
			Cartesian2dCoordinate result = converter.ConvertTo2d(source);

			// 3. Verify
			GeometryAssert.SamePoint(expected, result);
		}

		[Theory]
		[InlineData("(1 0 0 0)", "(0 1 0 -1)", "(0 0 0)", "(1 0)")]
		[InlineData("(1 0 0 0)", "(0 1 0 1)", "(0 0 0)", "(-1 0)")]
		[InlineData("(0 0 -1 -1)", "(0 1 0 -1)", "(0.95 0.95 -1)", "(1 0.95)")] // 1
		[InlineData("(0 0 -1 -1)", "(1 0 0 -1)", "(0.95 0.95 -1)", "(0.95 1)")] // 2
		[InlineData("(0 0 -1 -1)", "(0 -1 0 -1)", "(0.95 0.95 -1)", "(-1 0.95)")] // 4
		[InlineData("(0 0 -1 -1)", "(-1 0 0 -1)", "(0.95 0.95 -1)", "(0.95 -1)")] // 7
		[InlineData("(0 0 -1 -1)", "(1 -1 1 0)", "(0.95 0.95 -1)", "(0.4499999999999999999999999 1.45)")] // 3
		[InlineData("(0 0 -1 -1)", "(-1 -1 1 0)", "(0.95 0.95 -1)", "(-0.5 -0.5)")] // 5
		[InlineData("(0 0 -1 -1)", "(1 1 1 0)", "(0.95 0.95 -1)", "(0.5 0.5)")] // 6
		[InlineData("(0 0 -1 -1)", "(-1 1 1 0)", "(0.95 0.95 -1)", "(1.45 0.4499999999999999999999999)")] // 8
		public void CartesianCoordinatesFlattener_GetClosestPoint_BeExpected(string cPlane, string intersectionPlane, string c3d, string c2d)
		{
			// 1. Prepare
			Plane p = new Plane(cPlane);
			CartesianCoordinatesFlattener converter = new CartesianCoordinatesFlattener(p);
			Cartesian2dCoordinate expected = new Cartesian2dCoordinate(c2d);
			Cartesian3dCoordinate reference = new Cartesian3dCoordinate(c3d);
			Plane intersection = new Plane(intersectionPlane);

			// 2. Execute
			Cartesian2dCoordinate result = converter.GetClosestPoint(intersection, reference);

			// 3. Verify
			GeometryAssert.SamePoint(expected, result);
		}
		
		#endregion Test Methods
	}
}
