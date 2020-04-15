using System;
using Twisty.Engine.Geometry;
using Twisty.Engine.Geometry.Rotations;
using Xunit;

namespace Twisty.Engine.Tests.Geometry.Rotations
{
    [Trait("Category", "Geometry-Rotations")]
    public class RotationMatrix3dTest
    {
        private const int PRECISION_DOUBLE = 10;

        [Theory]
        // No rotation.
        [InlineData("(1 0 0)", "(1 0 0)", 0.0, "(1 0 0)")]
        [InlineData("(1 0 0)", "(1 0 0)", Math.PI * 2.0, "(1 0 0)")]
        [InlineData("(1 1 1)", "(1 0 0)", 0.0, "(1 1 1)")]
        [InlineData("(1 1 1)", "(1 0 0)", Math.PI * 2.0, "(1 1 1)")]
        [InlineData("(1 1 1)", "(1 1 1)", 0.0, "(1 1 1)")]
        [InlineData("(1 1 1)", "(1 1 1)", Math.PI * 2.0, "(1 1 1)")]
        // 90 degree rotations
        [InlineData("(1 0 0)", "(0 0 1)", Math.PI / 2.0, "(0 -1 0)")]
        [InlineData("(1 0 0)", "(0 0 -1)", Math.PI / 2.0, "(0 1 0)")]
        [InlineData("(1 0 0)", "(0 1 0)", Math.PI / 2.0, "(0 0 1)")]
		[InlineData("(1 1 1)", "(1 0 0)", Math.PI / 2.0, "(1 1 -1)")]
		[InlineData("(1 1 1)", "(0 1 0)", Math.PI / 2.0, "(-1 1 1)")]
		[InlineData("(1 1 1)", "(0 0 1)", Math.PI / 2.0, "(1 -1 1)")]
		public void RotationMatrix3d_RotateRotateVector_Expected(string origin, string axis, double angle, string expected)
        {
            // Prepare
            Cartesian3dCoordinate o = new Cartesian3dCoordinate(origin);
            Cartesian3dCoordinate a = new Cartesian3dCoordinate(axis);
            Cartesian3dCoordinate e = new Cartesian3dCoordinate(expected);
            RotationMatrix3d m = new RotationMatrix3d(a, angle);

            // Execute
            Cartesian3dCoordinate r = m.Rotate(o);

            // Verify
            Assert.Equal(e.X, r.X, PRECISION_DOUBLE);
            Assert.Equal(e.Y, r.Y, PRECISION_DOUBLE);
            Assert.Equal(e.Z, r.Z, PRECISION_DOUBLE);
        }

		[Theory]
		// No rotation.
		[InlineData("(1 0 0)", "(1 0 0)", 0.0, "(1 0 0)")]
		[InlineData("(1 0 0)", "(1 0 0)", Math.PI * 2.0, "(1 0 0)")]
		[InlineData("(1 1 1)", "(1 0 0)", 0.0, "(1 1 1)")]
		[InlineData("(1 1 1)", "(1 0 0)", Math.PI * 2.0, "(1 1 1)")]
		[InlineData("(1 1 1)", "(1 1 1)", 0.0, "(1 1 1)")]
		[InlineData("(1 1 1)", "(1 1 1)", Math.PI * 2.0, "(1 1 1)")]
		// 90 degree rotations
		[InlineData("(1 0 0)", "(0 0 1)", Math.PI / 2.0, "(-1 0 0)")]
		[InlineData("(1 0 0)", "(0 0 -1)", Math.PI / 2.0, "(-1 0 0)")]
		[InlineData("(1 0 0)", "(0 1 0)", Math.PI / 2.0, "(-1 0 0)")]
		[InlineData("(1 1 1)", "(1 0 0)", Math.PI / 2.0, "(1 -1 -1)")]
		[InlineData("(1 1 1)", "(0 1 0)", Math.PI / 2.0, "(-1 1 -1)")]
		[InlineData("(1 1 1)", "(0 0 1)", Math.PI / 2.0, "(-1 -1 1)")]
		public void RotationMatrix3d_SumSameRotationAndRotateVector_Expected(string origin, string axis, double angle, string expected)
		{
			// Prepare
			Cartesian3dCoordinate o = new Cartesian3dCoordinate(origin);
			Cartesian3dCoordinate a = new Cartesian3dCoordinate(axis);
			Cartesian3dCoordinate e = new Cartesian3dCoordinate(expected);
			RotationMatrix3d first = new RotationMatrix3d(a, angle);

			// Execute
			RotationMatrix3d m = first.Rotate(first);
			Cartesian3dCoordinate r = m.Rotate(o);

			// Verify
			Assert.Equal(e.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(e.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(e.Z, r.Z, PRECISION_DOUBLE);
		}

		[Theory]
		// 2 * 90 degree rotations
		[InlineData("(1 0 0)", "(0 0 1)", Math.PI / 2.0, "(1 0 0)", Math.PI / 2.0, "(0 0 1)")]
		[InlineData("(0 1 0)", "(0 0 1)", Math.PI / 2.0, "(1 0 0)", Math.PI / 2.0, "(1 0 0)")]
		[InlineData("(0 0 1)", "(0 0 1)", Math.PI / 2.0, "(1 0 0)", Math.PI / 2.0, "(0 1 0)")]
		[InlineData("(1 0 0)", "(0 0 1)", Math.PI / 2.0, "(0 0 1)", Math.PI / 2.0, "(-1 0 0)")]
		public void RotationMatrix3d_SumDifferentRotationAndRotateVector_Expected(string origin, string axis, double angle, string axis2, double angle2, string expected)
		{
			// Prepare
			Cartesian3dCoordinate o = new Cartesian3dCoordinate(origin);
			Cartesian3dCoordinate a = new Cartesian3dCoordinate(axis);
			Cartesian3dCoordinate a2 = new Cartesian3dCoordinate(axis2);
			Cartesian3dCoordinate e = new Cartesian3dCoordinate(expected);
			RotationMatrix3d first = new RotationMatrix3d(a, angle);
			RotationMatrix3d second = new RotationMatrix3d(a2, angle2);

			// Execute
			RotationMatrix3d m = first.Rotate(second);
			Cartesian3dCoordinate r = m.Rotate(o);

			// Verify
			Assert.Equal(e.X, r.X, PRECISION_DOUBLE);
			Assert.Equal(e.Y, r.Y, PRECISION_DOUBLE);
			Assert.Equal(e.Z, r.Z, PRECISION_DOUBLE);
		}
	}
}