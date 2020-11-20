using System;

namespace Twisty.Engine.Geometry.Rotations
{
	/// <summary>
	/// Class describing a Rotation using the rotation matrix formula.
	/// </summary>
	public class RotationMatrix3d
	{
		private readonly double[,] m_Matrix;

		/// <summary>
		/// Create a unrotated rotation matrix.
		/// </summary>
		public RotationMatrix3d()
		{
			this.m_Matrix = new double[,] { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };
		}

		/// <summary>
		/// Create a new rotation matrix based on a rotation axis and an angle.
		/// </summary>
		/// <param name="axis">Axis around which the rotation is executed</param>
		/// <param name="angle">Angle of the clockwise rotation.</param>
		public RotationMatrix3d(Cartesian3dCoordinate axis, double angle)
		{
			// Angle is reverse to provide a clockwise rotation.
			double c = Trigonometry.Cos(-angle);
			double s = Trigonometry.Sin(-angle);
			double t = 1.0 - c;

			// Need to use normalised vector.
			axis = axis.Normalize();

			// Create the matrix
			m_Matrix = new double[3, 3];

			m_Matrix[0, 0] = (c + (axis.X * axis.X * t)).AlignRatioLimits();
			m_Matrix[1, 1] = (c + (axis.Y * axis.Y * t)).AlignRatioLimits();
			m_Matrix[2, 2] = (c + (axis.Z * axis.Z * t)).AlignRatioLimits();

			double part1 = axis.X * axis.Y * t;
			double part2 = axis.Z * s;
			m_Matrix[1, 0] = (part1 + part2).AlignRatioLimits();
			m_Matrix[0, 1] = (part1 - part2).AlignRatioLimits();

			part1 = axis.X * axis.Z * t;
			part2 = axis.Y * s;
			m_Matrix[2, 0] = (part1 - part2).AlignRatioLimits();
			m_Matrix[0, 2] = (part1 + part2).AlignRatioLimits();

			part1 = axis.Y * axis.Z * t;
			part2 = axis.X * s;
			m_Matrix[2, 1] = (part1 + part2).AlignRatioLimits();
			m_Matrix[1, 2] = (part1 - part2).AlignRatioLimits();
		}

		/// <summary>
		/// Create a new rotation matrix based on a the matrix as a table format.
		/// </summary>
		/// <param name="matrix">Precalculated matrix as a table format.</param>
		private RotationMatrix3d(double[,] matrix)
		{
			if (matrix == null)
				throw new ArgumentNullException(nameof(matrix));

			if (matrix.Length != 9)
				throw new ArgumentException("Matrix size is expected to be [3, 3].");

			this.m_Matrix = matrix;
		}

		/// <summary>
		/// Rotate the provided vector based using this rotation.
		/// </summary>
		/// <param name="cc">Cartesian COordiante of the point to rotate.</param>
		/// <returns>Rotated value.</returns>
		public Cartesian3dCoordinate Rotate(Cartesian3dCoordinate cc)
		{
			return new Cartesian3dCoordinate(
				(cc.X * m_Matrix[0, 0]) + (cc.Y * m_Matrix[0, 1]) + (cc.Z * m_Matrix[0, 2]),
				(cc.X * m_Matrix[1, 0]) + (cc.Y * m_Matrix[1, 1]) + (cc.Z * m_Matrix[1, 2]),
				(cc.X * m_Matrix[2, 0]) + (cc.Y * m_Matrix[2, 1]) + (cc.Z * m_Matrix[2, 2]));
		}

		/// <summary>
		/// Rotate the provided vector based using this rotation.
		/// </summary>
		/// <param name="cc">Cartesian Coordinate of the point to rotate.</param>
		/// <returns>Rotated value.</returns>
		public RotationMatrix3d Rotate(RotationMatrix3d cc)
		{
			return new RotationMatrix3d(
				new double[3, 3]
				{
					{
						(cc.m_Matrix[0, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 0]),
						(cc.m_Matrix[0, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 1]),
						(cc.m_Matrix[0, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 2])
					},
					{
						(cc.m_Matrix[1, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 0]),
						(cc.m_Matrix[1, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 1]),
						(cc.m_Matrix[1, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 2])
					},
					{
						(cc.m_Matrix[2, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 0]),
						(cc.m_Matrix[2, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 1]),
						(cc.m_Matrix[2, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 2])
					}
				}
			);
		}
	}
}