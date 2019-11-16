using System;
using System.Diagnostics;

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
	///	        front view                       top view 
	/// 
	///	           0.0
	///	            *
	///	            |
	///	            |
	///	            O                               O
	/// 
	/// 2) Horizontal axis, defined by Phi, indicate the horizontal angle in the circle around the center.
	/// This angle is directive starting at the center.
	/// The Horizontal angle is providing us an 2D slice of the sphere in the form of a half circle
	/// 
	///	        front view                       top view                   front view without verical
	/// 
	/// 
	///            0,0                             90,90                             90,*
	///             *                                *                                 *
	///             |                                |                                 |   *
	///   180,90  270,90  0,90             180,90    |     0,90                        |     *
	///	     *------*------*                  *------O------*                          O------*
	///	            |                                |                                 |     *
	///	            |                                |                                 |   *
	///	            *                                *                                 *
	///	                                           270,90
	/// 
	/// 3) Vertical axis, defined by Theta, indicate the vertical angle inside the horizontal slice of the cude.
	/// As 2 opposites slices have different Horizontal angle, the vertical angle is limited to 180 degree.
	/// 
	///	        front view
	/// 
	///            0,0
	///             *   90,30
	///             |   *
	///             | /   90,90
	///	            O------*
	///	            | \
	///	            |   *
	///	            *  90,150
	///	           0,180
	///	           
	/// </example>
	/// <remarks>
	/// Note that this class is following the ISO convention.
	/// When used in mathematics theory, it can happen that the meaning of Phi and Theta are swapped together.
	/// </remarks>
	[DebuggerDisplay("({Phi},{Theta})")]
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
		/// <param name="phi">Azimuthal angle of the coordinates expressed in degree</param>
		/// <param name="theta">Polar angle of the coordinates expressed in degree.</param>
		/// <remarks>
		/// If angle value is supperior to 360 or negative, the value will be simplified to the equivalent between 0 and 360.
		/// Afterwards, if the value of Y is still superior to 180, the Orientation will review the value of X and Y to access the same axis with an Y value under 180.
		/// In this way, each point in the globe will only be reachable with one and only one Orientation coordonate.
		/// </remarks>
		public SphericalVector(int phi, int theta)
			: this(phi * (Math.PI / 180), theta * (Math.PI / 180))
		{
		}

		/// <summary>
		/// Create a new SphericalCoordinate with the provided angles.
		/// </summary>
		/// <param name="phi">Azimuthal angle of the coordinates expressed in variant.</param>
		/// <param name="theta">Polar angle of the coordinates expressed in variant.</param>
		/// <remarks>
		/// If angle value is supperior to 360 or negative, the value will be simplified to the equivalent between 0 and 360.
		/// Afterwards, if the value of Y is still superior to 180, the Orientation will review the value of X and Y to access the same axis with an Y value under 180.
		/// In this way, each point in the globe will only be reachable with one and only one Orientation coordonate.
		/// </remarks>
		public SphericalVector(double phi, double theta)
		{
			this.Phi = NormalizeValue(phi);
			this.Theta = NormalizeValue(theta);

			if (AreEqual(this.Theta, 0.0) || AreEqual(this.Theta, HALF_ANGLE))
				// All point with Theta in 180 and 0 join in the same point for the vertical axis, so keep a single Phi value.
				this.Phi = 0.0;
			else if (this.Theta > HALF_ANGLE)
			{
				// If Theta is bigger than the half circle, we need to match Phi to the opposite half circle.
				this.Phi = NormalizeValue(HALF_ANGLE + this.Phi);

				// Calculate the mirror value for Theta relative to the new Phi value.
				this.Theta = this.Theta - (this.Theta - HALF_ANGLE) * 2.0;
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
		public bool IsOnOrigin => AreEqual(0.0, this.Phi) && AreEqual(0.0, this.Theta);

		public SphericalVector RotateAround(SphericalVector v, double theta)
		{
			if (AreEqual(theta, 0.0))
				return this;

			return CoordinateConverter.ConvertToSpherical(CoordinateConverter.ConvertToCartesian(this).RotateAroundVector(CoordinateConverter.ConvertToCartesian(v), theta));
		}

		#region Operators Overrides

		/// <summary>
		/// Compare 2 Orientation class instance to ensure they are equals.
		/// </summary>
		/// <param name="o1">First object to compare.</param>
		/// <param name="o2">Second object to compare.</param>
		/// <returns>A boolean indicating whether the 2 objects are equals or not.</returns>

		public static bool operator ==(SphericalVector o1, SphericalVector o2)
		{
			return o1.Theta == o2.Theta && o1.Phi == o2.Phi;
		}

		/// <summary>
		/// Compare 2 Orientation class instance to ensure they are different.
		/// </summary>
		/// <param name="o1">First object to compare.</param>
		/// <param name="o2">Second object to compare.</param>
		/// <returns>A boolean indicating whether the 2 objects are different or not.</returns>
		public static bool operator !=(SphericalVector o1, SphericalVector o2)
		{
			return o1.Theta != o2.Theta || o1.Phi != o2.Phi;
		}

		#endregion Operators Overrides

		#region Objects overrides

		/// <summary>
		/// Provide the standard Hash code for this object.
		/// </summary>
		/// <returns>A general hascode unique to this object.</returns>
		public override int GetHashCode()
		{
			return (Theta + Phi * 10000.0).GetHashCode();
		}

		/// <summary>
		/// Determine whether the specified object is equals to the current object or not.
		/// </summary>
		/// <param name="obj">The object to compare to the current object.</param>
		/// <returns>A boolean indicating whther the 2 objects are equals or not.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is SphericalVector))
				return false;

			SphericalVector o = (SphericalVector)obj;
			return o.Theta == this.Theta && o.Phi == this.Phi;
		}

		#endregion Objects overrides

		#region Private Methods

		/// <summary>
		/// Evaluate if the 2 doubles are concidered as equal in this context.
		/// </summary>
		/// <param name="d1">First double value to compare.</param>
		/// <param name="d2">Second value to compare.</param>
		/// <returns>A boolean indicating whether the 2 doubles are equals or not.</returns>
		private bool AreEqual(double d1, double d2)
		{
			return Math.Abs(d1 - d2) < 0.0000000001;
		}

		/// <summary>
		/// Normalize the angle value to his expression with a value between 0 and 360.
		/// </summary>
		/// <param name="x">Angle value to normalize.</param>
		/// <returns>Normalized angle between 0 and 360.</returns>
		private static double NormalizeValue(double x)
		{
			if (x > 0.0)
				return x % MAX_ANGLE;

			if (x < 0.0)
				return MAX_ANGLE + (x % -MAX_ANGLE);

			return x;
		}

		#endregion Private Methods
	}
}
