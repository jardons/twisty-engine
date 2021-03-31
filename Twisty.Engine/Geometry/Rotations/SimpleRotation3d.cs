using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Geometry.Rotations
{
	/// <summary>
	/// Class describing a single rotation around an unique axis.
	/// </summary>
	public class SimpleRotation3d
	{
		/// <summary>
		/// Create a new rotation matrix based on a rotation axis and an angle.
		/// </summary>
		/// <param name="axis">Axis around which the rotation is executed.</param>
		/// <param name="angle">Angle of the clockwise rotation.</param>
		public SimpleRotation3d(Cartesian3dCoordinate axis, double angle)
		{
			if (axis.IsZero)
				throw new ArgumentException("Axis shoulb de directed.", nameof(axis));

			this.Axis = axis;
			this.Angle = angle;
		}

		/// <summary>
		/// Angle of the clockwise rotation.
		/// </summary>
		public double Angle { get; }

		/// <summary>
		/// Axis around which the rotation is executed.
		/// </summary>
		public Cartesian3dCoordinate Axis { get; }
	}
}
