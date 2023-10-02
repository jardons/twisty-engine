using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class defining the Spherical Vector of a vector in a 3D environement relative to a central point.
	/// THe coordinate calculation follow the specifications as defined in the ISO convention.
	/// </summary>
	/// <example>
	/// Visual example of the coordonate (in degree) :
	/// ==============================================
	/// 
	/// 1) Base Axis is vertical and directed to the top.
	/// 
	///         front view                       top view 
	/// 
	///            0.0
	///             *
	///             |
	///             |
	///             O                               O
	/// 
	/// 2) Horizontal axis, defined by Phi, indicate the horizontal angle in the circle around the center.
	/// This angle is directive starting at the center.
	/// The Horizontal angle is providing us an 2D slice of the sphere in the form of a half circle
	/// 
	///         front view                       top view                   front view without verical
	/// 
	/// 
	///            0,0                            180,90                             90,*
	///             *                                *                                 *
	///             |                                |                                 |   *
	///   180,90  270,90  0,90             270,90    |    90,90                        |     *
	///      *------*------*                  *------O------*                          O------*
	///             |                                |                                 |     *
	///             |                                |                                 |   *
	///             *                                *                                 *
	///                                             0,90
	/// 
	/// 3) Vertical axis, defined by Theta, indicate the vertical angle inside the horizontal slice of the cude.
	/// As 2 opposites slices have different Horizontal angle, the vertical angle is limited to 180 degree.
	/// 
	///         front view
	/// 
	///            0,0
	///             *   90,30
	///             |   *
	///             | /   90,90
	///             O------*
	///             | \
	///             |   *
	///             *  90,150
	///            0,180
	///            
	/// </example>
	/// <remarks>
	/// Note that this class is following the ISO convention.
	/// When used in mathematics theory, it can happen that the meaning of Phi and Theta are swapped together.
	/// </remarks>
	[DebuggerDisplay("({Phi}, {Theta})")]
	public struct SphericalVector
	{
		#region Const Members

		/// <summary>
		/// Define the maximal angle for a full circle.
		/// </summary>
		private const double MAX_ANGLE = 2 * Math.PI;

		/// <summary>
		/// Define the maximal angle of the Half circle.
		/// </summary>
		private const double HALF_ANGLE = MAX_ANGLE / 2;

		#endregion Const Members

		/// <summary>
		/// Gets the origin point coordinates.
		/// </summary>
		public static readonly SphericalVector Origin = new SphericalVector(0.0, 0.0);

		/// <summary>
		/// Create a new SphericalCoordinate with the provided angles.
		/// </summary>
		/// <param name="phi">Azimuthal angle of the coordinates expressed in variant.</param>
		/// <param name="theta">Polar angle of the coordinates expressed in variant.</param>
		/// <remarks>
		/// If angle value is supperior to Pi * 2 or negative, the value will be simplified to the equivalent between 0 and Pi * 2.
		/// Afterwards, if the value of Y is still superior to Pi, the Orientation will review the value of X and Y to access the same axis with an Y value under Pi.
		/// In this way, each point in the globe will only be reachable with one and only one Orientation coordonate.
		/// </remarks>
		public SphericalVector(double phi, double theta)
		{
			this.Phi = NormalizeValue(phi);
			this.Theta = NormalizeValue(theta);

			if (this.Theta.IsZero() || this.Theta.IsEqualTo(HALF_ANGLE))
				// All point with Theta in Pi and 0 join in the same point for the vertical axis, so keep a single Phi value.
				this.Phi = 0.0;
			else if (this.Theta > HALF_ANGLE)
			{
				// If Theta is bigger than the half circle, we need to match Phi to the opposite half circle.
				this.Phi = NormalizeValue(HALF_ANGLE + this.Phi);

				// Calculate the mirror value for Theta relative to the new Phi value.
				this.Theta = this.Theta - ((this.Theta - HALF_ANGLE) * 2.0);
			}
		}

		/// <summary>
		/// Define the azimuthal angle of the coordinates expressed in degree.
		/// </summary>
		public double Phi { get; }

		/// <summary>
		/// Define the polar angle of the coordinates expressed in degree.
		/// </summary>
		public double Theta { get; }

		/// <summary>
		/// Gets a boolean indicating whether the Vector is direct on the origin (0,0) coordinate or not.
		/// </summary>
		public readonly bool IsOnOrigin => this.Phi.IsZero() && this.Theta.IsZero();

		/// <summary>
		/// Rotate the current vector around a specified vector from a defined angle in radians.
		/// </summary>
		/// <param name="v">Vector used as a rotation axis.</param>
		/// <param name="theta">Angle of rotation expressed in radians.</param>
		/// <returns>The coordinates of the resulting rotated vector.</returns>
		public readonly SphericalVector RotateAround(SphericalVector v, double theta)
		{
			if (theta.IsZero())
				return this;

			return CoordinateConverter.ConvertToSpherical(CoordinateConverter.ConvertToCartesian(this).RotateAroundVector(CoordinateConverter.ConvertToCartesian(v), theta));
		}

		#region Private Methods

		/// <summary>
		/// Normalize the angle value to his expression with a value between 0 and 360.
		/// </summary>
		/// <param name="x">Angle value to normalize.</param>
		/// <returns>Normalized angle between 0 and 360.</returns>
		private static double NormalizeValue(double x)
		{
			// Catch back calculation precisions when needed.
			if (x.IsZero())
				return 0.0;

			if (x > 0.0)
				return x % MAX_ANGLE;

			// If not Zero or bigger, then always smaller than 0.0.
			return MAX_ANGLE + (x % -MAX_ANGLE);
		}

		#endregion Private Methods
	}
}