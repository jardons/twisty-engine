using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Xunit;

namespace Twisty.Engine.Tests.Assertions
{
	public static class GeometryAssert
	{
		/// <summary>
		/// Precision level used when unit testing double precisions.
		/// </summary>
		public const int PRECISION_DOUBLE = 10;

		/// <summary>
		/// Verifies that two angle values are equal, taking into account various way to represent hte same angle.
		/// </summary>
		/// <param name="expectedAngle">The expected value.</param>
		/// <param name="angle">The value to be compared against.</param>
		public static void AngleEqual(double expectedAngle, double angle)
		{
			expectedAngle %= (Math.PI * 2.0);
			angle %= (Math.PI * 2.0);

			if (expectedAngle < 0.0)
				expectedAngle += Math.PI * 2.0;

			if (angle < 0.0)
				angle += Math.PI * 2.0;

			Assert.Equal(expectedAngle, angle, PRECISION_DOUBLE);
		}

		/// <summary>
		/// Verifies that two Cartesian3dCoordinate values are representing the same point.
		/// </summary>
		/// <param name="expectedAngle">The expected value.</param>
		/// <param name="angle">The value to be compared against.</param>
		public static void SamePoint(Cartesian3dCoordinate expected, Cartesian3dCoordinate value)
		{
			Assert.Equal(expected.X, value.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, value.Y, PRECISION_DOUBLE);
			Assert.Equal(expected.Z, value.Z, PRECISION_DOUBLE);
		}

		/// <summary>
		/// Verifies that two Cartesian2dCoordinate values are representing the same point.
		/// </summary>
		/// <param name="expectedAngle">The expected value.</param>
		/// <param name="angle">The value to be compared against.</param>
		public static void SamePoint(Cartesian2dCoordinate expected, Cartesian2dCoordinate value)
		{
			Assert.Equal(expected.X, value.X, PRECISION_DOUBLE);
			Assert.Equal(expected.Y, value.Y, PRECISION_DOUBLE);
		}

		/// <summary>
		/// Verifies that two Cartesian3dCoordinate values are representing the same vector.
		/// </summary>
		/// <param name="expectedAngle">The expected value.</param>
		/// <param name="angle">The value to be compared against.</param>
		public static void SameVector(Cartesian3dCoordinate expected, Cartesian3dCoordinate value)
			=> SamePoint(expected.Normalize(), value.Normalize());

		/// <summary>
		/// Verifies that two ParametricLine values are representing the same line.
		/// </summary>
		/// <param name="expectedAngle">The expected value.</param>
		/// <param name="line">The line to be compared against.</param>
		public static void SameLine(ParametricLine expected, ParametricLine line)
		{
			SameVector(expected.Vector, line.Vector);
			Assert.True(expected.Contains(line.Point));
		}
	}
}
