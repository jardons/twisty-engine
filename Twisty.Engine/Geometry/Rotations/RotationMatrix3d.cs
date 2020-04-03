namespace Twisty.Engine.Geometry.Rotations
{
	/// <summary>
	/// Class describing a Rotation using the rotation matrix formula.
	/// </summary>
	public class RotationMatrix3d
	{
		private readonly double[,] m_Matrix;

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

			m_Matrix[0, 0] = c + axis.X * axis.X * t;
			m_Matrix[1, 1] = c + axis.Y * axis.Y * t;
			m_Matrix[2, 2] = c + axis.Z * axis.Z * t;

			double part1 = axis.X * axis.Y * t;
			double part2 = axis.Z * s;
			m_Matrix[1, 0] = part1 + part2;
			m_Matrix[0, 1] = part1 - part2;

			part1 = axis.X * axis.Z * t;
			part2 = axis.Y * s;
			m_Matrix[2, 0] = part1 - part2;
			m_Matrix[0, 2] = part1 + part2;

			part1 = axis.Y * axis.Z * t;
			part2 = axis.X * s;
			m_Matrix[2, 1] = part1 + part2;
			m_Matrix[1, 2] = part1 - part2;
		}

		/// <summary>
		/// ROtate the provided vector based using this rotation.
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
	}
}