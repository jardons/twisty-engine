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
		public void RotationMatrix3d_Rotate_Expected(string origin, string axis, double angle, string expected)
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
    }
}