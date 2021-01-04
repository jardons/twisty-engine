using System;
using Twisty.Engine.Geometry;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Geometry.Rotations
{
	[Trait("Category", "Geometry-Rotations")]
	public class RotationMatrix3dTest
	{
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
		public void RotationMatrix3d_RotateVector_Expected(string origin, string axis, double angle, string expected)
		{
			// Prepare
			Cartesian3dCoordinate o = new Cartesian3dCoordinate(origin);
			Cartesian3dCoordinate a = new Cartesian3dCoordinate(axis);
			Cartesian3dCoordinate e = new Cartesian3dCoordinate(expected);
			RotationMatrix3d m = new RotationMatrix3d(a, angle);

			// Execute
			Cartesian3dCoordinate r = m.Rotate(o);

			// Verify
			GeometryAssert.SamePoint(e, r);
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
			GeometryAssert.SamePoint(e, r);
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
			GeometryAssert.SamePoint(e, r);
		}

		[Fact]
		public void RotationMatrix3d_GetEulerAnglesFromNonRotated_Empty()
		{
			// Prepare
			RotationMatrix3d m = new RotationMatrix3d();

			// Execute
			var r = m.GetEulerAngles();

			// Verify
			Assert.NotNull(r);
			Assert.Empty(r);
		}

		[Theory]
		// General cases.
		[InlineData("(1 0 0)", Math.PI)]
		[InlineData("(0 1 0)", Math.PI / 2.0)]
		[InlineData("(0 0 1)", Math.PI / 3.0)]
		[InlineData("(-1 0 0)", Math.PI / 13.0)]
		[InlineData("(0 -1 0)", Math.PI * 1.5)]
		[InlineData("(0 0 -1)", Math.PI / 3.0 * 2.0)]
		// Gimbal lock cases.
		[InlineData("(0 -1 0)", Math.PI / 2.0)]
		public void RotationMatrix3d_GetEulerAnglesFromGeneralAxisRotation_Expected(string axisCoordinates, double theta)
		{
			// Prepare
			Cartesian3dCoordinate axis = new Cartesian3dCoordinate(axisCoordinates);
			RotationMatrix3d m = new RotationMatrix3d(axis, theta);
			bool useReverse = axis.X + axis.Y + axis.Z < 0.0;
			Cartesian3dCoordinate expectedAxis = useReverse ? axis.Reverse : axis;
			double expectedTheta = useReverse ? -theta : theta;

			// Execute
			var r = m.GetEulerAngles();

			// Verify
			Assert.NotNull(r);
			Assert.Equal(1, r.Count);
			GeometryAssert.SameVector(expectedAxis, r[0].Axis);
			GeometryAssert.AngleEqual(expectedTheta, r[0].Angle);
		}

		[Theory]
		// General cases.
		[InlineData("(1 0 0)", Math.PI, "(0 0 1)", Math.PI / 2.0, Math.PI, double.NaN, Math.PI / 2.0)]
		// Gimbal lock cases.
		[InlineData("(1 0 0)", Math.PI, "(0 1 0)", Math.PI / 2.0, Math.PI, Math.PI / 2.0, double.NaN)]
		public void RotationMatrix3d_GetEulerAnglesFrom2GeneralAxisRotation_Expected(string axisCoordinates1, double theta1, string axisCoordinates2, double theta2,
			double expectedThetaX, double expectedThetaY, double expectedThetaZ)
		{
			// Prepare
			RotationMatrix3d m1 = new RotationMatrix3d(new Cartesian3dCoordinate(axisCoordinates1), theta1);
			RotationMatrix3d m2 = new RotationMatrix3d(new Cartesian3dCoordinate(axisCoordinates2), theta2);
			RotationMatrix3d rotated = m2.Rotate(m1);
			int expectedCount = 0;
			if (!double.IsNaN(expectedThetaX))
				++expectedCount;
			if (!double.IsNaN(expectedThetaY))
				++expectedCount;
			if (!double.IsNaN(expectedThetaZ))
				++expectedCount;

			// Execute
			var r = rotated.GetEulerAngles();

			// Verify
			Assert.NotNull(r);
			Assert.Equal(expectedCount, r.Count);
			foreach (var rotation in r)
			{
				Cartesian3dCoordinate? expectedAxis = null;
				double? expectedTheta = null;
				if (rotation.Axis.IsOnX)
				{
					expectedAxis = Cartesian3dCoordinate.XAxis;
					expectedTheta = expectedThetaX;
				}
				else if (rotation.Axis.IsOnY)
				{
					expectedAxis = Cartesian3dCoordinate.YAxis;
					expectedTheta = expectedThetaY;
				}
				else if (rotation.Axis.IsOnZ)
				{
					expectedAxis = Cartesian3dCoordinate.ZAxis;
					expectedTheta = expectedThetaZ;
				}

				Assert.NotNull(expectedAxis);
				Assert.NotNull(expectedTheta);
				GeometryAssert.SameVector((Cartesian3dCoordinate)expectedAxis, rotation.Axis);
				GeometryAssert.AngleEqual((double)expectedTheta, rotation.Angle);
			}
		}

		[Theory]
		// General cases.
		[InlineData("(1 1 1)", Math.PI * 2.0 / 3.0, Math.PI / 2.0, double.NaN, Math.PI / 2.0)]
		// Gimbal lock cases.
		[InlineData("(1 -1 1)", Math.PI * 2.0 / 3.0, Math.PI / 2.0, -Math.PI / 2.0, double.NaN)]
		public void RotationMatrix3d_GetEulerAnglesFromNonGeneralAxisRotation_Expected(string axisCoordinates, double theta,
			double expectedThetaX, double expectedThetaY, double expectedThetaZ)
		{
			// Prepare
			RotationMatrix3d m = new RotationMatrix3d(new Cartesian3dCoordinate(axisCoordinates), theta);
			int expectedCount = 0;
			if (!double.IsNaN(expectedThetaX))
				++expectedCount;
			if (!double.IsNaN(expectedThetaY))
				++expectedCount;
			if (!double.IsNaN(expectedThetaZ))
				++expectedCount;

			// Execute
			var r = m.GetEulerAngles();

			// Verify
			Assert.NotNull(r);
			Assert.Equal(expectedCount, r.Count);
			foreach (var rotation in r)
			{
				Cartesian3dCoordinate? expectedAxis = null;
				double? expectedTheta = null;
				if (rotation.Axis.IsOnX)
				{
					expectedAxis = Cartesian3dCoordinate.XAxis;
					expectedTheta = expectedThetaX;
				}
				else if (rotation.Axis.IsOnY)
				{
					expectedAxis = Cartesian3dCoordinate.YAxis;
					expectedTheta = expectedThetaY;
				}
				else if (rotation.Axis.IsOnZ)
				{
					expectedAxis = Cartesian3dCoordinate.ZAxis;
					expectedTheta = expectedThetaZ;
				}

				Assert.NotNull(expectedAxis);
				Assert.NotNull(expectedTheta);
				GeometryAssert.SameVector((Cartesian3dCoordinate)expectedAxis, rotation.Axis);
				GeometryAssert.AngleEqual((double)expectedTheta, rotation.Angle);
			}
		}
	}
}